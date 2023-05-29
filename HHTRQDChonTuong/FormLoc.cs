using HHTRQDChonTuong.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace HHTRQDChonTuong
{
    public partial class FormLoc : Form
    {
        public FormLoc()
        {

            InitializeComponent();
        }

        private void FormLoc_Load(object sender, EventArgs e)
        {
            loaDSTr();
            loadNganh();
            loadLSKQ();
        }

        private void loaDSTr()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            List<Truong> tr = context.Truong.ToList();
            dataGridViewDSTr.DataSource = tr;
            numericUpDown1.DecimalPlaces = 2;
            numericUpDown1.Increment = 0.1M;
            numericUpDownDiem2.DecimalPlaces = 2;
            numericUpDownDiem2.Increment = 0.1M;
            


        }
        public void loadLSKQ()
        {
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
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            List<LsketQua> tr = context.LsketQua
                .Where(x => x.Ipmac == macAddress)
                .Select(x => new LsketQua
                {
                    Ngay = x.Ngay,
                    MaTruong = x.MaTruong,
                    KetQua = x.KetQua,
                    Rank = x.Rank
                })
                .ToList();
            dataGridViewLSKQ.DataSource = tr;
            dataGridViewLSKQ.Columns["Id"].Visible = false;
            dataGridViewLSKQ.Columns["Ipmac"].Visible = false;
        }
        private void loadNganh()
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            List<Nganh> listNganh = context.Nganh.ToList();
            checkedListBoxNganh.DataSource = listNganh;
            checkedListBoxNganh.DisplayMember = "TenNganh";
            checkedListBoxNganh.ValueMember = "MaNganh";
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownHP1_ValueChanged(object sender, EventArgs e)
        {
            decimal value = numericUpDownHP1.Value;
            string formattedValue = value.ToString("N");
            numericUpDownHP1.Text = formattedValue;
        }
        private void locTruong(string manganh, float diem1, float diem2, float hocphi1,float hocphi2)
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            List<PhuThuoc> phuThuocs = context.PhuThuoc.Where(x => x.MaNganh == manganh && x.HocPhi >= hocphi1 && x.HocPhi <=hocphi2 
                                                              && x.Diem >= diem1 && x.Diem <=diem2).ToList();
            dataGridViewDSTr.DataSource = phuThuocs;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            string Diem1 = numericUpDown1.Value.ToString();
            float diem1 = Convert.ToSingle(Diem1);
            string Diem2 = numericUpDownDiem2.Value.ToString();
            float diem2 = Convert.ToSingle(Diem2);
            string HP1 = numericUpDownHP1.Value.ToString(); 
            float hp1 = Convert.ToSingle(HP1);
            string HP2 = numericUpDownHP2.Value.ToString();
            float hp2 = Convert.ToSingle(HP2);
            Nganh selectedNganh = (Nganh)checkedListBoxNganh.SelectedItem;
            string maNganh = selectedNganh.MaNganh;


            var query = from ct in context.PhuThuoc
                        where ct.Diem >= diem1 && ct.Diem <= diem2
                        && ct.HocPhi >= hp1 && ct.HocPhi <= hp2
                        && ct.MaNganh == maNganh
                        select ct.MaTruong;

            var distinctQuery = query.Distinct();

            var truongQuery = from t in context.Truong
                              where query.Contains(t.MaTruong)
                              select t;

            dataGridViewDSTr.DataSource = truongQuery.ToList();















        }

        private void buttonLuu_Click(object sender, EventArgs e)
        {
            List<string> listMaTruong = new List<string>();
            foreach (DataGridViewRow row in dataGridViewDSTr.Rows)
            {
                if (!row.IsNewRow)
                {
                    listMaTruong.Add(row.Cells["MaTruong"].Value.ToString());
                }
            }
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            context.TruongAhp.RemoveRange(context.TruongAhp);
            context.SaveChanges();
            string ip = GetLocalIPAddress();
            string ngay = DateTime.Now.ToString("yyyyMMdd");

            string key = ip + "_" + ngay;
            int i = 1; 
            foreach (string maTruong in listMaTruong)
            {
                i++;
                // Kiểm tra sự tồn tại của khóa chính
                var existingRecord = context.TruongAhp.FirstOrDefault(x => x.Id == key && x.MaTruong == maTruong);
                if (existingRecord == null)
                {
                    // Nếu không có thì thêm bản ghi mới vào
                    TruongAhp truongAHP = new TruongAhp()
                    {
                        MaTruong = maTruong,
                        HocPhi = GetHocPhi(maTruong),
                        CoSoVatChat = GetCoSoVatChat(maTruong),
                        CoHoiViecLam = GetCoHoiViecLam(maTruong),
                        HoatDongXaHoi = GetHoatDongXaHoi(maTruong),
                        Id = key+ i,


                    };
                    context.TruongAhp.Add(truongAHP);
                }
                else
                {
                    // Nếu có rồi thì cập nhật bản ghi đó
                    existingRecord.HocPhi = GetHocPhi(maTruong);
                    existingRecord.CoSoVatChat = GetCoSoVatChat(maTruong);
                    existingRecord.CoHoiViecLam = GetCoHoiViecLam(maTruong);
                    existingRecord.HoatDongXaHoi = GetHoatDongXaHoi(maTruong);
                }
            }

            context.SaveChanges();
            FormMasterY a = new FormMasterY();
            a.Show();
            FormLoc b = new FormLoc();
            b.Close();


        }
        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Không tìm thấy địa chỉ IP.");
        }
        private int GetHocPhi(string maTruong)
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            {
                return context.PhuThuoc.FirstOrDefault(x => x.MaTruong == maTruong)?.HocPhi ?? 0;
            }
        }
        private int GetCoSoVatChat(string maTruong)
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            
                return context.Truong.SingleOrDefault(x => x.MaTruong == maTruong)?.CoSoVatChat ?? 0;
            
        }
        private float GetCoHoiViecLam(string maTruong)
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();

            return (float)(context.Truong.SingleOrDefault(x => x.MaTruong == maTruong)?.CoHoiViecLam ?? 0);
            
        }
        private int GetHoatDongXaHoi(string maTruong)
        {
            HeHoTroRaQuyetDinhContext context = new HeHoTroRaQuyetDinhContext();
            
                return context.Truong.SingleOrDefault(x => x.MaTruong == maTruong)?.HoatDongXaHoi ?? 0;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkedListBoxNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCount = checkedListBoxNganh.CheckedItems.Count;

            if (selectedCount > 1)
            {
                // Nếu người dùng đã chọn nhiều hơn một ngành, hủy chọn ngành hiện tại
                checkedListBoxNganh.SetItemChecked(checkedListBoxNganh.SelectedIndex, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormAHP a = new FormAHP();
            a.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormAHPPA a = new FormAHPPA();
            a.ShowDialog();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
