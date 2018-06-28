﻿using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.NhaXes;
using Nop.Web.Models.VeXeKhach;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Models.NhaXes
{
    public class XeXuatBenItemModel : BaseNopEntityModel
    {

        public XeXuatBenItemModel()
        {
            laivaphuxes = new List<NhanVienLaiPhuXe>();
            nhatkys = new List<NhatKyXeXuatBen>();
            isEdit = true;
        }

        public int NguonVeId { get; set; }
        public int XeVanChuyenId { get; set; }
        public string BienSo { get; set; }
        public int TrangThaiId { get; set; }
        public ENTrangThaiXeXuatBen TrangThai
        {
            get
            {
                return (ENTrangThaiXeXuatBen)TrangThaiId;
            }
            set
            {
                TrangThaiId = (int)value;
            }
        }
        public string TrangThaiText { get; set; }

        public DateTime NgayDi { get; set; }
        public DateTime ThoiGianDi { get; set; }
        public int SoNguoi { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTaoId { get; set; }
        public string TenNguoiTao { get; set; }
        public string GhiChu { get; set; }
        public int HanhTrinhId { get; set; }
        public string TuyenXeChay { get; set; }

        public string GioDi { get; set; }
        public string GioDen { get; set; }

        public List<NhanVienLaiPhuXe> tatcalaivaphuxes { get; set; }
        public List<NhanVienLaiPhuXe> laivaphuxes { get; set; }
        public List<NhatKyXeXuatBen> nhatkys { get; set; }
        public List<PhieuGuiHangModel> phieuguihangs { get; set; }
        public bool isEdit { get; set; }
        public class NhanVienLaiPhuXe : BaseNopEntityModel
        {
            public NhanVienLaiPhuXe(int _id, string _thongtin)
            {
                Id = _id;
                ThongTin = _thongtin;
                TenLaiXe = _thongtin;
            }
            public string ThongTin { get; set; }
            public string TenLaiXe { get; set; }
        }
        public class NhatKyXeXuatBen : BaseNopEntityModel
        {
            public string GhiChu { get; set; }
            public DateTime NgayTao { get; set; }
            public int NguoiTaoId { get; set; }
            public string TenNguoiTao { get; set; }
        }
        public class XeVanChuyenInfo : BaseNopEntityModel
        {
            public XeVanChuyenInfo(int _id, string _bienso)
            {
                Id = _id;
                BienSo = _bienso;
            }
            public string BienSo { get; set; }
        }
    }
}