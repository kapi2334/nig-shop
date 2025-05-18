﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Models;

namespace UserService.Models{
    [Table("adres")]
    internal class Address{
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("ulica")]
        public string street { get; set; }

        [Column("nrdomu")]
        public int buildingNo { get; set; }

        [Column("nrmieszkania")]
        public int? localeNo { get; set; }

        [Column("kodpocztowy")]
        public string postCode { get; set; }

        [Column("miasto")]
        public string city { get; set; }

        [Column("kraj")]
        public string country { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}