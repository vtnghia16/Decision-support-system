using HHTRQDChonTuong.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HHTRQDChonTuong
{
    public partial class FormAHP : Form
    {
        public FormAHP()
        {
            InitializeComponent();
        }

        private void FormAHP_Load(object sender, EventArgs e)
        {
            loadTruongAHP();
            tieuChiAHP();
        }
        private void loadTruongAHP()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            List<TruongAhp> tr = context.TruongAhp.ToList();
            dataGridViewTrAHP.DataSource = tr;
            dataGridViewTrAHP.Columns["Id"].Visible = false;

        }
        private void tieuChiAHP()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            // Lấy danh sách các tiêu chí từ CSDL
            List<TieuChi> listTieuChi = context.TieuChi.ToList();

            // Thêm các cột vào DataGridView
            dataGridViewTC.Columns.Clear();
            dataGridViewTC.Rows.Clear();
            dataGridViewTC.Columns.Add("empty", "");
            dataGridViewTC.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // cột rỗng

            foreach (var tc in listTieuChi)
            {
                dataGridViewTC.Columns.Add(tc.TenTieuChi, tc.TenTieuChi);
                dataGridViewTC.Columns[dataGridViewTC.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dataGridViewTC.Rows.Add(listTieuChi.Count);

            // Thêm các dòng vào DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                dataGridViewTC.Rows[i].HeaderCell.Value = listTieuChi[i].TenTieuChi;
            }

            // Cho phép người dùng nhập giá trị đánh giá vào các ô bên trong DataGridView
            for (int i = 0; i < listTieuChi.Count; i++)
            {
                for (int j = 1; j <= listTieuChi.Count; j++)
                {
                    var cell = dataGridViewTC.Rows[j - 1].Cells[i + 1];
                    cell.Value = 1;
                    cell.ReadOnly = i == j - 1 || i < j - 1;
                    cell.Style.BackColor = i == j - 1 ? System.Drawing.SystemColors.Control : System.Drawing.SystemColors.Window;

                    if (i < j - 1)
                    {
                        var oppositeCell = dataGridViewTC.Rows[i].Cells[j];

                        dataGridViewTC.CellValueChanged += (sender, e) =>
                        {
                            int rowIndex = e.RowIndex;
                            int columnIndex = e.ColumnIndex;

                            // Kiểm tra chỉ thực hiện cập nhật giá trị đối diện khi thay đổi giá trị ở các ô phía trên đường chéo
                            if (columnIndex > rowIndex)
                            {
                                var currentCell = dataGridViewTC.Rows[rowIndex].Cells[columnIndex];

                                // Kiểm tra giá trị của ô vừa thay đổi
                                if (double.TryParse(currentCell.Value?.ToString(), out double currentValue))
                                {
                                    // Cập nhật giá trị đối diện
                                    var oppositeCell = dataGridViewTC.Rows[columnIndex - 1].Cells[rowIndex + 1];
                                    oppositeCell.Value = Math.Round(1 / currentValue, 4);
                                }
                            }

                            // Tính toán độ nhất quán
                            dataGridViewTC_CellValueChanged(sender, e);
                        };
                    }
                }
                


            }
        }

        private void dataGridViewTC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int n = dataGridViewTC.RowCount - 1;
            double[,] matrix = new double[n, n]; // Ma trận so sánh cặp các tiêu chí
           

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
                        matrix[i, j] = Convert.ToDouble(dataGridViewTC.Rows[i].Cells[j + 1].Value);
                        matrix[j, i] = 1 / matrix[i, j]; // Ma trận đối xứng
                    }
                }
            }

        

          

           


            // Gán DataTable cho DataSource của DataGridView

            // Tính tổng các cột trong ma trận và gán giá trị tổng vào cột tương ứng trong dataGridView3
            double[] columnSums = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += matrix[j, i];
                }
                columnSums[i] = sum;
            }

            // Xóa bỏ cột hiện có trong dataGridView3 (trừ cột đầu tiên là cột "Tiêu chí")
            while (dataGridView3.Columns.Count > 1)
            {
                dataGridView3.Columns.RemoveAt(1);
            }

            // Thêm các cột mới vào dataGridView3 dựa trên số lượng cột trong ma trận
            for (int i = 0; i < n; i++)
            {
                dataGridView3.Columns.Add("Cột " + (i + 1), "Cột " + (i + 1));
                dataGridView3.Rows[0].Cells[i].Value = Math.Round( columnSums[i],4);
            }
            // Tạo ma trận mới để lưu kết quả
            double[,] normalizedMatrix = new double[n, n];

            // Chia từng phần tử trong cột của ma trận cho tổng cột tương ứng
            for (int i = 0; i < n; i++)
            {
                double columnSum = columnSums[i];

                for (int j = 0; j < n; j++)
                {
                    normalizedMatrix[j, i] = matrix[j, i] / columnSum;
                }
            }

            // Xóa bỏ cột hiện có trong dataGridView4 (nếu có)
            dataGridView4.Columns.Clear();

            // Thêm các cột mới vào dataGridView4 dựa trên số lượng cột trong ma trận
            for (int i = 0; i < n; i++)
            {
                dataGridView4.Columns.Add("Cột " + (i + 1), "Cột " + (i + 1));
            }

            // Thêm các dòng vào dataGridView4 và gán giá trị từ ma trận mới
            for (int i = 0; i < n; i++)
            {
                dataGridView4.Rows.Add();

                for (int j = 0; j < n; j++)
                {
                    dataGridView4.Rows[i].Cells[j].Value = Math.Round (normalizedMatrix[i, j],4);
                }
            }
            // Tạo mảng chứa tổng các hàng
            double[] rowSums = new double[n];

            // Tính tổng các hàng và lưu vào mảng rowSums
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += normalizedMatrix[i, j];
                }
                rowSums[i] = sum / 4;
            }

            // Tạo bảng dữ liệu mới cho dataGridView5
            DataTable resultTable = new DataTable();

            // Thêm cột vào bảng dữ liệu
            resultTable.Columns.Add("Vector trọng số", typeof(double));

            // Thêm từng hàng vào bảng dữ liệu
            for (int i = 0; i < n; i++)
            {
                // Thêm giá trị của tổng hàng chia cho 4 vào bảng dữ liệu
                resultTable.Rows.Add(Math.Round(rowSums[i],4));
            }

            // Gán bảng dữ liệu cho DataSource của dataGridView5
            dataGridView5.DataSource = resultTable;
            /////////////////////////////////
            // Tạo ma trận kết quả
            double[,] resultMatrix = new double[n, n];

            // Nhân ma trận so sánh cặp các tiêu chí với vector trọng số
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    resultMatrix[i, j] = matrix[i, j] * rowSums[j];
                }
            }

            // Tạo bảng dữ liệu mới cho dataGridView1
            DataTable resultTable1 = new DataTable();

            // Thêm các cột vào bảng dữ liệu
            for (int i = 0; i < n; i++)
            {
                resultTable1.Columns.Add($"Cột {i + 1}", typeof(double));
            }

            // Thêm từng hàng vào bảng dữ liệu
            for (int i = 0; i < n; i++)
            {
                DataRow row = resultTable1.NewRow();
                for (int j = 0; j < n; j++)
                {
                    row[j] = Math.Round( resultMatrix[i, j],4);
                }
                resultTable1.Rows.Add(row);
            }

            // Gán bảng dữ liệu cho DataSource của dataGridView1
            dataGridView1.DataSource = resultTable1;

            ///////////////////////////////
            // Tính tổng trọng số của các tiêu chí
            double[] totalWeightVector = new double[n];

            // Tính tổng trọng số của các tiêu chí
            for (int i = 0; i < n; i++)
            {
                double totalWeight = 0;

                for (int j = 0; j < n; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        totalWeight += Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
                    }
                }

                totalWeightVector[i] = totalWeight;
            }

            // Tạo bảng dữ liệu mới cho dataGridView6
            DataTable resultTable2 = new DataTable();

            // Thêm cột vào bảng dữ liệu
            resultTable2.Columns.Add("Vector trọng số", typeof(double));

            // Thêm giá trị tổng trọng số vào bảng dữ liệu
            for (int i = 0; i < n; i++)
            {
                resultTable2.Rows.Add(Math.Round(totalWeightVector[i],4));
            }

            // Gán bảng dữ liệu cho DataSource của dataGridView6
            dataGridView6.DataSource = resultTable2;
            /////////////////////////////////////
            // Lấy vector tổng trọng số từ dataGridView6
            double[] totalWeightVector1 = new double[n];
            for (int i = 0; i < n; i++)
            {
                totalWeightVector1[i] = Convert.ToDouble(dataGridView6.Rows[i].Cells[0].Value);
            }

            // Lấy vector trọng số từ dataGridView5
            double[] weightVector = new double[n];
            for (int i = 0; i < n; i++)
            {
                weightVector[i] = Convert.ToDouble(dataGridView5.Rows[i].Cells[0].Value);
            }

            // Tạo mảng chứa vector nhất quán
            double[] consistencyVector = new double[n];

            // Tính vector nhất quán bằng cách chia vector tổng trọng số cho vector trọng số
            for (int i = 0; i < n; i++)
            {
                consistencyVector[i] = totalWeightVector1[i] / weightVector[i];
            }

            // Tạo bảng dữ liệu mới cho dataGridView7
            DataTable resultTable3 = new DataTable();

            // Thêm cột vào bảng dữ liệu
            resultTable3.Columns.Add("Vector nhất quán", typeof(double));

            // Thêm giá trị vector nhất quán vào bảng dữ liệu
            for (int i = 0; i < n; i++)
            {
                resultTable3.Rows.Add(Math.Round(consistencyVector[i],4));
            }

            // Gán bảng dữ liệu cho DataSource của dataGridView7
            dataGridView7.DataSource = resultTable3;

            ////////////////////////////////////
            // Tạo mảng chứa vector nhất quán từ dataGridView7
            double[] consistencyVector1 = new double[n];
            for (int i = 0; i < n; i++)
            {
                consistencyVector1[i] = Convert.ToDouble(dataGridView7.Rows[i].Cells[0].Value);
            }

            // Tính giá trị lambda max bằng trung bình cộng của vector nhất quán
            double lambdaMax = consistencyVector1.Average();

            // Hiển thị kết quả lambda max trong textBoxLmax
            textBoxLmax.Text = Math.Round(lambdaMax,4).ToString();
            //////////////////////////////// Tính chỉ số nhất quán
            double lambdaMax1 = double.Parse(textBoxLmax.Text);

            // Tính chỉ số nhất quán (Consistency Index - CI)
            double CI = (lambdaMax1 - n) / (n - 1);

            // Hiển thị kết quả CI trong textBoxCSNQ
            textBoxCSNQ.Text = Math.Round(CI,4).ToString();
            ///////////////////////// Tính tỷ số nhất quán 
            double CI1 = double.Parse(textBoxCSNQ.Text);

            // Tính tỷ số nhất quán (Consistency Ratio - CR)
            double CR = CI1 / 0.9;

            // Hiển thị kết quả CR trong textBoxCSCR
            textBoxCSCR.Text = Math.Round(CR,4).ToString();
            if (CR > 0.1)
            {
                textBoxCSCR.ForeColor = Color.Red;
            }
            else
            {
                textBoxCSCR.ForeColor = SystemColors.ControlText; // Màu chữ mặc định
            }
            //////////////////////////////////////////////////////

            double[] rowSums1 = new double[n];
            string[] tieuChiValues = { "học phí", "cơ hội việc làm", "cơ sở vật chất", "hoạt động xã hội" };

            // Tính tổng các hàng và lưu vào mảng rowSums
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += normalizedMatrix[i, j];
                }
                rowSums1[i] = sum / 4;
            }

            // Tạo bảng dữ liệu mới cho dataGridView8
            DataTable resultTable4 = new DataTable();

            // Thêm cột vào bảng dữ liệu
            resultTable4.Columns.Add("Tiêu chí", typeof(string));
            resultTable4.Columns.Add("Vector trọng số", typeof(double));
            resultTable4.Columns.Add("Rank", typeof(int));

            // Sắp xếp mảng rowSums giảm dần và gán rank tương ứng
            int[] ranks = rowSums1.Select((value, index) => new { Value = value, Index = index })
                                .OrderByDescending(x => x.Value)
                                .Select((x, rank) => new { Index = x.Index, Rank = rank + 1 })
                                .OrderBy(x => x.Index)
                                .Select(x => x.Rank)
                                .ToArray();

            // Thêm từng hàng vào bảng dữ liệu
            for (int i = 0; i < n; i++)
            {
                // Thêm giá trị của tổng hàng chia cho 4, giá trị rank và giá trị tiêu chí tương ứng vào bảng dữ liệu
                resultTable4.Rows.Add(tieuChiValues[i], Math.Round(rowSums1[i], 4), ranks[i]);
            }

            dataGridView8.DataSource = resultTable4;









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


        private double ComputeConsistencyRatio(double consistencyIndex, int n)
        {
            // Lấy giá trị Random Index (RI) tương ứng với số tiêu chí n
            double[] randomIndices = { 0, 0, 0.58, 0.90, 1.12, 1.24, 1.32, 1.41, 1.45, 1.49 };

            // Kiểm tra nếu giá trị n không hợp lệ (không có trong randomIndices)
            if (n < 1 || n > 10)
            {
                // Nếu n không hợp lệ, trả về giá trị -1 để biểu thị không xác định
                return -1;
            }

            // Tính chỉ số độ nhất quán ngẫu nhiên (Random Index - RI)
            double randomIndex = randomIndices[n - 1];

            // Tính chỉ số độ nhất quán ngẫu nhiên (Consistency Ratio - CR)
            double consistencyRatio = consistencyIndex / randomIndex;

            return consistencyRatio;
        }

        private void CheckConsistency()
        {
            int n = dataGridViewTC.RowCount - 1;
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
                        matrix[i, j] = Convert.ToDouble(dataGridViewTC.Rows[i].Cells[j + 1].Value);
                        matrix[j, i] = 1 / matrix[i, j]; // Ma trận đối xứng
                    }
                }
            }

            double consistencyIndex = ComputeConsistencyIndex(matrix);
            double consistencyRatio = ComputeConsistencyRatio(consistencyIndex, n);

            // Hiển thị kết quả
            textBoxCI.Text = consistencyIndex.ToString();
            textBoxCR.Text = consistencyRatio.ToString();

            // Kiểm tra và thông báo độ nhất quán
            if (consistencyRatio <= 0.1)
            {
                MessageBox.Show("Ma trận có độ nhất quán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ma trận không nhất quán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonTT_Click(object sender, EventArgs e)
        {
            FormAHPPA a = new FormAHPPA();
            a.ShowDialog();
            //}
            //double[,] BuildDecisionMatrix(int maTieuChi)
            //{
            //    HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            //    int numTruong = context.TruongAhp.Select(a => a.MaTruong).Distinct().Count();
            //    double[,] decisionMatrix = new double[numTruong, numTruong];

            //    // Lấy danh sách các trường
            //    List<string> dsTruong = context.TruongAhp.Select(a => a.MaTruong).Distinct().ToList();

            //    // Tính phần trăm cho mỗi trường đối với tiêu chí đang xét
            //    double[] phanTram = new double[numTruong];
            //    for (int i = 0; i < numTruong; i++)
            //    {
            //        phanTram[i] = (from a in context.TruongAhp
            //                       join b in context.TieuChi on a.MaTieuChi equals b.MaTieuChi
            //                       where a.MaTruong == dsTruong[i] && b.MaTieuChi == maTieuChi
            //                       select a.TyLe).FirstOrDefault();
            //    }

            //    // Tạo ma trận phương án
            //    for (int i = 0; i < numTruong; i++)
            //    {
            //        for (int j = 0; j < numTruong; j++)
            //        {
            //            if (i == j)
            //            {
            //                decisionMatrix[i, j] = 1.0;
            //            }
            //            else if (i < j)
            //            {
            //                double value = phanTram[i] / phanTram[j];
            //                decisionMatrix[i, j] = value;
            //                decisionMatrix[j, i] = 1.0 / value;
            //            }
            //        }
            //    }

            //    return decisionMatrix;
        }

        private void dataGridViewTC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonKT_Click(object sender, EventArgs e)
        {
            CheckConsistency();

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
