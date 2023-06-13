using HHTRQDChonTuong.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace HHTRQDChonTuong
{
    public partial class FormMasterY : Form
    {
        public FormMasterY()
        {
            InitializeComponent();
        }

        private void btnTinhToan_Click(object sender, EventArgs e)
        {
            CheckConsistency();
        }
        private void CheckConsistency()
        {
            int n = tableTrongSoTieuChi.RowCount - 1;
            double[,] matrix = new double[n, n];

            // Lấy giá trị trong DataGridView để cập nhật ma trận so sánh cặp các tiêu chí
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 1; // Đường chéo chính bằng 1
                    }
                    else
                    {
                        matrix[i, j] = Convert.ToDouble(tableTrongSoTieuChi.Rows[i].Cells[j].Value);
                        matrix[j, i] = 1 / matrix[i, j]; // Ma trận đối xứng
                    }
                }
            }
            double[] columnSums = TinhTongCacCot(tableTrongSoTieuChi);
            double[,] matrix4 = LayMaTranChiaChoTongCacCot(tableTrongSoTieuChi, columnSums);
            double[,] matrix1 = RemoveLastRowFromMatrix(matrix4);
            double[] vectorTS= TinhTrungBinhCongHang(matrix1);
            double[,] matrix2 = ConvertDataGridViewToMatrix(tableTrongSoTieuChi);
            double[,] matrix3 = NhanMaTranVoiVector(matrix2, vectorTS);
            double[] tonghang = CalculateRowSums(matrix3);
            double[] vectorT = VectorDivision(tonghang,vectorTS);
            double lamda = CalculateAverage(vectorT);
            double CI = CalculateCI(lamda,n);
            double CR = CalculateCR(CI);
            List<string> danhSachTenTruong = LayDanhSachTenTruong();

            // Chuyển danh sách tên trường thành một vector
            string[] vectorTenTruong = danhSachTenTruong.ToArray();
            if (CR > 0.1)
            {

                MessageBox.Show("Không có sự nhất quán, vui lòng nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            else
            {

                double[] vectorPAHP = loadPAHP();
                double[] vectorPATLVL = loadPATLVL();
                double[] vectorPCSVL = loadPCSVL();
                double[] vectorHDXH = loadHDXH();
                double[,] resultMatrix = GopVectorsThanhMaTran(vectorPAHP, vectorPATLVL, vectorPCSVL, vectorHDXH);
                double[] ketqua = MultiplyMatrixByVector(resultMatrix, vectorTS);
                System.Data.DataTable matranketqua = CreateMatrixWithRank(vectorTenTruong,ketqua);
                dataGridViewKQ.DataSource = matranketqua;
                DisplayColumnSumsOnDataGridView(ketqua,dataGridView3);

                // Sử dụng kết quả tính được

            }


        }
        private System.Data.DataTable CreateMatrixWithRank(string[] vectorTenTruong, double[] ketqua)
        {
            int length = vectorTenTruong.Length;

            // Create DataTable with columns
            System.Data.DataTable matrixTable = new System.Data.DataTable();
            matrixTable.Columns.Add("TenTruong", typeof(string));
            matrixTable.Columns.Add("Ketqua", typeof(double));
            matrixTable.Columns.Add("Rank", typeof(int));

            // Populate the DataTable with values
            for (int i = 0; i < length; i++)
            {
                matrixTable.Rows.Add(vectorTenTruong[i], ketqua[i], i + 1);
            }

            // Sort the DataTable based on the Ketqua column in descending order
            DataView sortedView = matrixTable.DefaultView;
            sortedView.Sort = "Ketqua DESC";
            System.Data.DataTable sortedMatrixTable = sortedView.ToTable();

            // Update the Rank column based on the sorted order
            for (int i = 0; i < length; i++)
            {
                sortedMatrixTable.Rows[i]["Rank"] = i + 1;
            }

            return sortedMatrixTable;
        }




        private void loadTruongAHP()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            List<TruongAhp> tr = context.TruongAhp.ToList();
            dataGridView1.DataSource = tr;
            dataGridView1.Columns["Id"].Visible = false;
        }
        private List<string> LayDanhSachTenTruong()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();

            List<TruongAhp> listTruongAhp = context.TruongAhp.ToList();

            List<string> danhSachTenTruong = new List<string>();

            foreach (var truongAhp in listTruongAhp)
            {
                danhSachTenTruong.Add(truongAhp.MaTruong);
            }

            return danhSachTenTruong;
        }

        private double[] MultiplyMatrixByVector(double[,] matrix, double[] vector)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            if (columnCount != vector.Length)
            {
                throw new ArgumentException("The number of columns in the matrix must be equal to the length of the vector.");
            }

            double[] result = new double[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                double sum = 0;

                for (int j = 0; j < columnCount; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }

                result[i] = sum;
            }

            return result;
        }

       
        private double ComputeConsistencyIndex(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[] weightVector = new double[n];

            // Tính vector trọng số các tiêu chí
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += matrix[j, i];
                }
                weightVector[i] = sum / n;
            }

            // Tính tổng các phần tử trên đường chéo chính
            double diagonalSum = 0;
            for (int i = 0; i < n; i++)
            {
                diagonalSum += matrix[i, i];
            }

            // Tính chỉ số độ nhất quán (Consistency Index - CI)
            double consistencyIndex = (diagonalSum - n) / (n - 1);

            return consistencyIndex;
        }

        private void FormMasterY_Load(object sender, EventArgs e)
        {
            tieuChiAHP();
            loadTruongAHP();
            tableTrongSoTieuChi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void tieuChiAHP()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            // Lấy danh sách các tiêu chí từ CSDL
            List<TieuChi> listTieuChi = context.TieuChi.ToList();

            // Thêm các cột vào DataGridView
            tableTrongSoTieuChi.Columns.Clear();
            tableTrongSoTieuChi.Rows.Clear();

            foreach (var tc in listTieuChi)
            {
                tableTrongSoTieuChi.Columns.Add(tc.TenTieuChi, tc.TenTieuChi);
            }

            tableTrongSoTieuChi.Rows.Add(listTieuChi.Count);

            // Thêm các dòng vào DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                tableTrongSoTieuChi.Rows[i].HeaderCell.Value = listTieuChi[i].TenTieuChi;
                tableTrongSoTieuChi.Rows[i].HeaderCell.Style.WrapMode = DataGridViewTriState.True;
            }

            // Cho phép người dùng nhập giá trị đánh giá vào các ô bên trong DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                for (int j = 0; j < listTieuChi.Count; j++)
                {
                    var cell = tableTrongSoTieuChi.Rows[j].Cells[i];
                    if (i == j)
                    {
                        cell.Value = 1;
                    }
                    else
                    {
                        cell.Value = "";
                    }
                    cell.ReadOnly = i == j || i < j;
                    cell.Style.BackColor = i == j ? SystemColors.Control : SystemColors.Window;

                    if (i < j)
                    {
                        var oppositeCell = tableTrongSoTieuChi.Rows[i].Cells[j];

                        tableTrongSoTieuChi.CellValueChanged += (sender, e) =>
                        {
                            int rowIndex = e.RowIndex;
                            int columnIndex = e.ColumnIndex;

                            // Kiểm tra chỉ thực hiện cập nhật giá trị đối diện khi thay đổi giá trị ở các ô phía trên đường chéo
                            if (columnIndex > rowIndex)
                            {
                                var currentCell = tableTrongSoTieuChi.Rows[rowIndex].Cells[columnIndex];

                                // Kiểm tra giá trị của ô vừa thay đổi
                                if (double.TryParse(currentCell.Value?.ToString(), out double currentValue))
                                {
                                    // Cập nhật giá trị đối diện
                                    var oppositeCell = tableTrongSoTieuChi.Rows[columnIndex].Cells[rowIndex];
                                    oppositeCell.Value = Math.Round(1 / currentValue, 4);
                                }
                            }

                        };
                    }
                }
            }
          
        }
        private void HienThiMaTranLenDataGridView(DataGridView dataGridView, double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            // Thêm các cột vào DataGridView
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                dataGridView.Columns.Add($"Column{columnIndex + 1}", $"Column {columnIndex + 1}");
            }

            // Thêm các dòng vào DataGridView và gán giá trị
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                dataGridView.Rows.Add();

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    double value = matrix[rowIndex, columnIndex];
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Value = value;
                }
            }
        }

        private void DisplayColumnSumsOnDataGridView(double[] columnSums, DataGridView dataGridView)
        {
            int rowCount = columnSums.Length;

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            // Thêm cột vào dataGridView
            dataGridView.Columns.Add("ColumnSums", "Column Sums");

            // Thêm dòng vào dataGridView và gán giá trị từ mảng columnSums
            for (int i = 0; i < rowCount; i++)
            {
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = columnSums[i];
            }
        }
        private double[] TinhTongCacCot(DataGridView dataGridView)
        {
            int rowCount = dataGridView.RowCount;
            int columnCount = dataGridView.ColumnCount;

            double[] columnSums = new double[columnCount];

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
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

                columnSums[columnIndex] = sum;
            }

            return columnSums;
        }
        private double[,] LayMaTranChiaChoTongCacCot(DataGridView dataGridView, double[] columnSums)
        {
            int rowCount = dataGridView.RowCount;
            int columnCount = dataGridView.ColumnCount;

            double[,] matrix = new double[rowCount, columnCount];

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    var cellValue = dataGridView.Rows[rowIndex].Cells[columnIndex].Value;

                    if (cellValue != null && double.TryParse(cellValue.ToString(), out double cellNumber))
                    {
                        // Chia giá trị của ô cho tổng cột tương ứng
                        matrix[rowIndex, columnIndex] = cellNumber / columnSums[columnIndex];
                    }
                    else
                    {
                        matrix[rowIndex, columnIndex] = 0; // Giá trị không hợp lệ, có thể xử lý hoặc bỏ qua tùy theo yêu cầu
                    }
                }
            }

            return matrix;
        }
        private double[,] RemoveLastRowFromMatrix(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            if (rowCount < 1)
            {
                throw new ArgumentException("The matrix must have at least one row.");
            }

            double[,] newMatrix = new double[rowCount - 1, columnCount];

            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }

            return newMatrix;
        }

        private double[] TinhTrungBinhCongHang(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            double[] rowAverages = new double[rowCount];

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                double sum = 0;

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    sum += matrix[rowIndex, columnIndex];
                }

                rowAverages[rowIndex] = sum / columnCount;
            }

            return rowAverages;
        }
        private double[,] NhanMaTranVoiVector(double[,] matrix, double[] vector)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            double[,] result = new double[rowCount - 1, columnCount];

            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    result[i, j] = matrix[i, j] * vector[j];
                }
            }

            return result;
        }


        private double[,] ConvertDataGridViewToMatrix(DataGridView dataGridView)
        {
            int rowCount = dataGridView.RowCount;
            int columnCount = dataGridView.ColumnCount;

            double[,] matrix = new double[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    var cellValue = dataGridView.Rows[i].Cells[j].Value;

                    if (cellValue != null && double.TryParse(cellValue.ToString(), out double cellNumber))
                    {
                        matrix[i, j] = cellNumber;
                    }
                    else
                    {
                        // Giá trị không hợp lệ, có thể xử lý hoặc bỏ qua tùy theo yêu cầu
                        matrix[i, j] = 0;
                    }
                }
            }

            return matrix;
        }

        private double[] CalculateRowSums(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int columnCount = matrix.GetLength(1);

            double[] rowSums = new double[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                double sum = 0;
                for (int j = 0; j < columnCount; j++)
                {
                    sum += matrix[i, j];
                }
                rowSums[i] = sum;
            }

            return rowSums;
        }
        private double[] VectorDivision(double[] vector1, double[] vector2)
        {
            int length = vector1.Length;

            double[] result = new double[length];

            for (int i = 0; i < length; i++)
            {
                if (vector2[i] != 0)
                {
                    result[i] = vector1[i] / vector2[i];
                }
                else
                {
                    // Xử lý trường hợp chia cho 0 tùy theo yêu cầu
                    // Ví dụ: Gán giá trị NaN, 0 hoặc xử lý báo lỗi
                    result[i] = 0; // Giả sử gán giá trị 0 cho trường hợp chia cho 0
                }
            }

            return result;
        }

        private double CalculateAverage(double[] vector)
        {
            int length = vector.Length;

            if (length == 0)
            {
                return 0; // Trường hợp vector rỗng, trả về giá trị mặc định
            }

            double sum = 0;

            for (int i = 0; i < length; i++)
            {
                sum += vector[i];
            }

            double average = sum / length;

            return average;
        }
        private double CalculateCI(double lambda, int n)
        {
            return (lambda - n) / (n - 1);
        }
        private double CalculateCR(double ci)
        {
            return ci / 0.9;
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

        private void tableTrongSoTieuChi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableTrongSoTieuChi_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = tableTrongSoTieuChi.RowCount - 1;
            double[,] matrix = new double[n, n];

            // Lấy giá trị trong DataGridView để cập nhật ma trận so sánh cặp các tiêu chí
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 1; // Đường chéo chính bằng 1
                    }
                    else
                    {
                        matrix[i, j] = Convert.ToDouble(tableTrongSoTieuChi.Rows[i].Cells[j].Value);
                        matrix[j, i] = 1 / matrix[i, j]; // Ma trận đối xứng
                    }
                }
            }
            double[] columnSums = TinhTongCacCot(tableTrongSoTieuChi);
            double[,] matrix4 = LayMaTranChiaChoTongCacCot(tableTrongSoTieuChi, columnSums);
            double[,] matrix1 = RemoveLastRowFromMatrix(matrix4);
            double[] vectorTS = TinhTrungBinhCongHang(matrix1);
            double[,] matrix2 = ConvertDataGridViewToMatrix(tableTrongSoTieuChi);
            double[,] matrix3 = NhanMaTranVoiVector(matrix2, vectorTS);
            double[] tonghang = CalculateRowSums(matrix3);
            double[] vectorT = VectorDivision(tonghang, vectorTS);
            double lamda = CalculateAverage(vectorT);
            double CI = CalculateCI(lamda, n);
            double CR = CalculateCR(CI);

            List<string> danhSachTenTruong = LayDanhSachTenTruong();
            double[] vectorPAHP = loadPAHP();
            double[] vectorPATLVL = loadPATLVL();
            double[] vectorPCSVL = loadPCSVL();
            double[] vectorHDXH = loadHDXH();
            double[,] resultMatrix = GopVectorsThanhMaTran(vectorPAHP, vectorPATLVL, vectorPCSVL, vectorHDXH);
            double[] ketqua = MultiplyMatrixByVector(resultMatrix, vectorTS);
            // Chuyển danh sách tên trường thành một vector
            string[] vectorTenTruong = danhSachTenTruong.ToArray();
            System.Data.DataTable matranketqua = CreateMatrixWithRank(vectorTenTruong, ketqua);
            string macAddress = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            // Create a list to hold the LsketQua entities
            List<LsketQua> lsketQuaList = new List<LsketQua>();

            // Iterate over the rows of the DataTable
            foreach (DataRow row in matranketqua.Rows)
            {
                // Create a new LsketQua entity and populate its properties from the DataTable row
                LsketQua lsketQua = new LsketQua
                {
                    Ipmac = macAddress,
                    Ngay = DateTime.Now,
                    MaTruong = row["TenTruong"].ToString(),
                    KetQua = Convert.ToDouble(row["Ketqua"]),
                    Rank = Convert.ToInt32(row["Rank"])
                };

                // Add the entity to the list
                lsketQuaList.Add(lsketQua);
            }

            // Save the list of LsketQua entities to the database
            using (var context = new HeHoTroRaQuyetDinhContext())
            {
                context.LsketQua.AddRange(lsketQuaList);
                context.SaveChanges();
            }

            // Show a message indicating the successful save
            MessageBox.Show("Data saved to the database.");
            FormLoc mainForm = System.Windows.Forms.Application.OpenForms.OfType<FormLoc>().FirstOrDefault();
            if (mainForm != null)
            {
                mainForm.loadLSKQ();
                // Gọi lại hàm load lịch sử form chính
            }
        }

        private void btnAHPPA_Click(object sender, EventArgs e)
        {
            FormAHPPA f = new FormAHPPA();
            f.ShowDialog();
        }
    }
}
