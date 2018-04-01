using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Ip { get; set; }
        public string Qq { get; set; }
        public string Email { get; set; }
        public string HomePage { get; set; }
        public sbyte? IsChecked { get; set; }
        public sbyte? IsDel { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
