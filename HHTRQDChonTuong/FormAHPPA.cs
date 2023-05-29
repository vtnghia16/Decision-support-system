using HHTRQDChonTuong.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HHTRQDChonTuong
{
    public partial class FormAHPPA : Form
    {
        public FormAHPPA()
        {
            InitializeComponent();
        }

        private void FormAHPPA_Load(object sender, EventArgs e)
        {
            double[] vectorPAHP = loadPAHP();
            double[] vectorPATLVL = loadPATLVL();
            double[] vectorPCSVL = loadPCSVL();
            double[] vectorHDXH = loadHDXH();
            double[,] resultMatrix = GopVectorsThanhMaTran(vectorPAHP, vectorPATLVL, vectorPCSVL, vectorHDXH);
            HienThiMaTranLenDataGridView(resultMatrix, dataGridView3);

        }
        private void HienThiMaTranLenDataGridView(double[,] matrix, DataGridView dataGridView)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            for (int j = 0; j < columnCount; j++)
            {
                dataGridView.Columns.Add("Column" + j, "Column" + j);
            }

            for (int i = 0; i < rowCount; i++)
            {
                DataGridViewRow row = new DataGridViewRow();

                for (int j = 0; j < columnCount; j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = matrix[i, j]
                    });
                }

                dataGridView.Rows.Add(row);
            }
        }

        private double[,] GopVectorsThanhMaTran(double[] vector1, double[] vector2, double[] vector3, double[] vector4)
        {
            int rowCount = vector1.Length;
            int columnCount = 4; // Số lượng vector

            double[,] matrix = new double[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                matrix[i, 0] = vector1[i];
                matrix[i, 1] = vector2[i];
                matrix[i, 2] = vector3[i];
                matrix[i, 3] = vector4[i];
            }

            return matrix;
        }

        private double[] loadPAHP()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            // Lấy danh sách các tiêu chí từ CSDL
            List<TruongAhp> listTieuChi = context.TruongAhp.ToList();

            // Thêm các cột vào DataGridView
            dataGridViewHP.Columns.Clear();
            dataGridViewHP.Rows.Clear();
            dataGridViewHP.Columns.Add("empty", "");
            dataGridViewHP.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // cột rỗng

            foreach (var tc in listTieuChi)
            {
                dataGridViewHP.Columns.Add(tc.MaTruong, tc.MaTruong);
                dataGridViewHP.Columns[dataGridViewHP.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewHP.Rows.Add(listTieuChi.Count);

            // Thêm các dòng vào DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                dataGridViewHP.Rows[i].HeaderCell.Value = listTieuChi[i].MaTruong;
            }

            // Gắn giá trị tỷ lệ học phí vào ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewHP.Rows[i - 1].Cells[j];

                        var truongA = listTieuChi[i - 1];
                        var truongB = listTieuChi[j - 1];

                        if (truongA.HocPhi.HasValue && truongA.HocPhi.Value != 0 && truongB.HocPhi.HasValue && truongB.HocPhi.Value != 0)
                        {
                            double tyLeHocPhi = truongA.HocPhi.Value / truongB.HocPhi.Value;
                            cell.Value = Math.Round(tyLeHocPhi, 4);
                        }
                        else
                        {
                            cell.Value = DBNull.Value; // Hoặc giá trị mặc định khác tuỳ bạn muốn
                        }
                    }
                    else
                    {
                        var cell = dataGridViewHP.Rows[i - 1].Cells[j];
                        cell.Value = 1;
                    }
                }
            }

            // Vô hiệu hóa chỉnh sửa các ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewHP.Rows[i - 1].Cells[j];
                        cell.ReadOnly = true;
                        cell.Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
            double[] columnSums = TongCacCot(dataGridViewHP);
            double[,] matrix = MaTranDGChiaTong(dataGridViewHP, columnSums);
            double[] vector = TinhTrungBinhCongHang(matrix);
            //// Hiển thị ma trận lên dataGridView1
            //DisplayMatrixOnDataGridView(matrix, dataGridView1);
            //// Hiển thị mảng columnSums lên dataGridView2
            //DisplayColumnSumsOnDataGridView(columnSums, dataGridView2);
            //HienThiKetQuaTrungBinhCong(dataGridView3, vector);
            return vector;

        }
    
        private double[] TinhTrungBinhCongHang(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            double[] rowAverages = new double[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                double sum = 0;
                int count = 0;

                for (int j = 0; j < columnCount; j++)
                {
                    if (!double.IsNaN(matrix[i, j]))
                    {
                        sum += matrix[i, j];
                        count++;
                    }
                }

                rowAverages[i] = count > 0 ? sum / count : 0;
            }

            return rowAverages;
        }
        private double[,] MaTranDGChiaTong(DataGridView dataGridView, double[] columnSums)
        {
            int rowCount = dataGridView.RowCount - 1; // Loại bỏ hàng cuối cùng
            int columnCount = dataGridView.ColumnCount - 1; // Loại bỏ cột rỗng đầu tiên

            double[,] matrix = new double[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    var cell = dataGridView.Rows[i].Cells[j];
                    double value = 0;

                    if (cell.Value != null && double.TryParse(cell.Value.ToString(), out value))
                    {
                        matrix[i, j - 1] = Math.Round(value, 4);
                    }
                    else
                    {
                        // Giá trị không hợp lệ, có thể xử lý hoặc bỏ qua tùy theo yêu cầu
                        matrix[i, j - 1] = 0;
                    }
                }
            }

            // Chia ma trận cho mảng columnSums
            double[,] result = MaTranChiaTong(matrix, columnSums);

            return result;
        }

        private double[,] MaTranChiaTong(double[,] matrix, double[] columnSums)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            double[,] result = new double[rowCount, columnCount];

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    result[rowIndex, columnIndex] = matrix[rowIndex, columnIndex] / columnSums[columnIndex];
                }
            }

            return result;
        }

        private double[] TongCacCot(DataGridView dataGridView)
{
    int rowCount = dataGridView.RowCount;
    int columnCount = dataGridView.ColumnCount;

    double[] columnSums = new double[columnCount - 1]; // Giảm kích thước mảng đi 1 vì loại bỏ cột đầu tiên

    for (int columnIndex = 1; columnIndex < columnCount; columnIndex++) // Bắt đầu từ columnIndex = 1 để loại bỏ cột đầu tiên
    {
        double sum = 0;

        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
        {
            var cellValue = dataGridView.Rows[rowIndex].Cells[columnIndex].Value;

            if (cellValue != null && double.TryParse(cellValue.ToString(), out double cellNumber))
            {
                sum += cellNumber;
            }
        }

        columnSums[columnIndex - 1] = sum; // Giảm chỉ số mảng đi 1 vì loại bỏ cột đầu tiên
    }

    // Loại bỏ các giá trị trống cuối cùng của mảng
    while (columnSums.Length > 0 && columnSums[columnSums.Length - 1] == 0)
    {
        Array.Resize(ref columnSums, columnSums.Length - 1);
    }

    return columnSums;
}




        private double[] loadPATLVL()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            // Lấy danh sách các tiêu chí từ CSDL
            List<TruongAhp> listTieuChi = context.TruongAhp.ToList();

            // Thêm các cột vào DataGridView
            dataGridViewTLVL.Columns.Clear();
            dataGridViewTLVL.Rows.Clear();
            dataGridViewTLVL.Columns.Add("empty", "");
            dataGridViewTLVL.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // cột rỗng

            foreach (var tc in listTieuChi)
            {
                dataGridViewTLVL.Columns.Add(tc.MaTruong, tc.MaTruong);
                dataGridViewTLVL.Columns[dataGridViewTLVL.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewTLVL.Rows.Add(listTieuChi.Count);

            // Thêm các dòng vào DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                dataGridViewTLVL.Rows[i].HeaderCell.Value = listTieuChi[i].MaTruong;
            }

            // Gắn giá trị tỷ lệ học phí vào ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewTLVL.Rows[i - 1].Cells[j];

                        var truongA = listTieuChi[i - 1];
                        var truongB = listTieuChi[j - 1];

                        if (truongA.CoHoiViecLam.HasValue && truongA.CoHoiViecLam.Value != 0 && truongB.CoHoiViecLam.HasValue && truongB.CoHoiViecLam.Value != 0)
                        {
                            double tyLeHocPhi = truongA.CoHoiViecLam.Value / truongB.CoHoiViecLam.Value;
                            cell.Value = Math.Round(tyLeHocPhi, 4);
                        }
                        else
                        {
                            cell.Value = DBNull.Value; // Hoặc giá trị mặc định khác tuỳ bạn muốn
                        }
                    }
                    else
                    {
                        var cell = dataGridViewTLVL.Rows[i - 1].Cells[j];
                        cell.Value = 1;
                    }
                }
            }

            // Vô hiệu hóa chỉnh sửa các ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewTLVL.Rows[i - 1].Cells[j];
                        cell.ReadOnly = true;
                        cell.Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
            double[] columnSums = TongCacCot(dataGridViewTLVL);
            double[,] matrix = MaTranDGChiaTong(dataGridViewTLVL, columnSums);
            double[] vector = TinhTrungBinhCongHang(matrix);
            return vector;
        }

        private double[] loadPCSVL()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            // Lấy danh sách các tiêu chí từ CSDL
            List<TruongAhp> listTieuChi = context.TruongAhp.ToList();

            // Thêm các cột vào DataGridView
            dataGridViewCSVC.Columns.Clear();
            dataGridViewCSVC.Rows.Clear();
            dataGridViewCSVC.Columns.Add("empty", "");
            dataGridViewCSVC.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // cột rỗng

            foreach (var tc in listTieuChi)
            {
                dataGridViewCSVC.Columns.Add(tc.MaTruong, tc.MaTruong);
                dataGridViewCSVC.Columns[dataGridViewCSVC.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewCSVC.Rows.Add(listTieuChi.Count);

            // Thêm các dòng vào DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                dataGridViewCSVC.Rows[i].HeaderCell.Value = listTieuChi[i].MaTruong;
            }

            // Gắn giá trị tỷ lệ học phí vào ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewCSVC.Rows[i - 1].Cells[j];

                        var truongA = listTieuChi[i - 1];
                        var truongB = listTieuChi[j - 1];

                        if (truongA.CoSoVatChat.HasValue && truongA.CoSoVatChat.Value != 0 && truongB.CoSoVatChat.HasValue && truongB.CoSoVatChat.Value != 0)
                        {
                            double tyLeHocPhi = truongA.CoSoVatChat.Value / truongB.CoSoVatChat.Value;
                            cell.Value = Math.Round(tyLeHocPhi, 4);
                        }
                        else
                        {
                            cell.Value = DBNull.Value; // Hoặc giá trị mặc định khác tuỳ bạn muốn
                        }
                    }
                    else
                    {
                        var cell = dataGridViewCSVC.Rows[i - 1].Cells[j];
                        cell.Value = 1;
                    }
                }
            }

            // Vô hiệu hóa chỉnh sửa các ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewCSVC.Rows[i - 1].Cells[j];
                        cell.ReadOnly = true;
                        cell.Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
            double[] columnSums = TongCacCot(dataGridViewCSVC);
            double[,] matrix = MaTranDGChiaTong(dataGridViewCSVC, columnSums);
            double[] vector = TinhTrungBinhCongHang(matrix);
            //// Hiển thị ma trận lên dataGridView1
            //DisplayMatrixOnDataGridView(matrix, dataGridView1);
            //// Hiển thị mảng columnSums lên dataGridView2
            //DisplayColumnSumsOnDataGridView(columnSums, dataGridView2);
            //HienThiKetQuaTrungBinhCong(dataGridView3, vector);

            return vector;
        }
        private double[] loadHDXH()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            // Lấy danh sách các tiêu chí từ CSDL
            List<TruongAhp> listTieuChi = context.TruongAhp.ToList();

            // Thêm các cột vào DataGridView
            dataGridViewHDXH.Columns.Clear();
            dataGridViewHDXH.Rows.Clear();
            dataGridViewHDXH.Columns.Add("empty", "");
            dataGridViewHDXH.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // cột rỗng

            foreach (var tc in listTieuChi)
            {
                dataGridViewHDXH.Columns.Add(tc.MaTruong, tc.MaTruong);
                dataGridViewHDXH.Columns[dataGridViewHDXH.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewHDXH.Rows.Add(listTieuChi.Count);

            // Thêm các dòng vào DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                dataGridViewHDXH.Rows[i].HeaderCell.Value = listTieuChi[i].MaTruong;
            }

            // Gắn giá trị tỷ lệ học phí vào ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewHDXH.Rows[i - 1].Cells[j];

                        var truongA = listTieuChi[i - 1];
                        var truongB = listTieuChi[j - 1];

                        if (truongA.HoatDongXaHoi.HasValue && truongA.HoatDongXaHoi.Value != 0 && truongB.HoatDongXaHoi.HasValue && truongB.HoatDongXaHoi.Value != 0)
                        {
                            double tyLeHocPhi = truongA.HoatDongXaHoi.Value / truongB.HoatDongXaHoi.Value;
                            cell.Value = Math.Round(tyLeHocPhi, 4);
                        }
                        else
                        {
                            cell.Value = DBNull.Value; // Hoặc giá trị mặc định khác tuỳ bạn muốn
                        }
                    }
                    else
                    {
                        var cell = dataGridViewHDXH.Rows[i - 1].Cells[j];
                        cell.Value = 1;
                    }
                }
            }

            // Vô hiệu hóa chỉnh sửa các ô giao nhau
            for (int i = 1; i <= listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    if (i != j)
                    {
                        var cell = dataGridViewHDXH.Rows[i - 1].Cells[j];
                        cell.ReadOnly = true;
                        cell.Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
            double[] columnSums = TongCacCot(dataGridViewHDXH);
            double[,] matrix = MaTranDGChiaTong(dataGridViewHDXH, columnSums);
            double[] vector = TinhTrungBinhCongHang(matrix);
            //// Hiển thị ma trận lên dataGridView1
            //DisplayMatrixOnDataGridView(matrix, dataGridView1);
            //// Hiển thị mảng columnSums lên dataGridView2
            //DisplayColumnSumsOnDataGridView(columnSums, dataGridView2);
            //HienThiKetQuaTrungBinhCong(dataGridView3, vector);

            return vector;
        }

    }
}
