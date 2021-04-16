using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.ViewModels
{
    public class RoomRequestModel
    {
        [Required(ErrorMessage = "Tên phòng bắt buộc nhập")]
        public string Name { get; set; }

        [Range(2, 9, ErrorMessage = "Số lượng người chơi từ 2 người đến 9 người")]
        public int NumberOfPlayers { get; set; }

        [Range(1000, long.MaxValue, ErrorMessage = "Đặt cược tối thiểu 1000 VNĐ")]
        public long MinimumBet { get; set; }

        [Range(1000, long.MaxValue, ErrorMessage = "Đặt cược tối đa phải hơn 1000 VNĐ")]
        public long ExpectedMaximumBet { get; set; }
    }
}
