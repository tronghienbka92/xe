using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;

namespace Nop.Data.Mapping.NhaXes
{
    public class HopDongChuyenMap : NopEntityTypeConfiguration<HopDongChuyen>
    {
        public HopDongChuyenMap()
        {
            this.ToTable("CV_HopDongChuyen");
            this.HasKey(c => c.Id);
            this.HasRequired(c => c.XeInfo)
              .WithMany()
              .HasForeignKey(c => c.XeVanChuyenId);
            this.HasOptional(c => c.laixe)
             .WithMany()
             .HasForeignKey(c => c.LaiXeId);
            this.Ignore(c => c.TrangThai);
            this.Ignore(c => c.HinhThucThanhToan);


        }
    }
}
