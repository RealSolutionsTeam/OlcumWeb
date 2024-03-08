namespace OlcumVeriAktarimi.dbAtolye
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        public int Id { get; set; }

        public string KullaniciAd { get; set; }

        public string KullaniciName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public int? Rol { get; set; }

        public string Sifre { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
    }
}
