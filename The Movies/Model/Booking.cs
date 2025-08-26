using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Booking
    {
        private int _phone;
        private string _email;
        private int _qtyTickets;
        private Show _show;


        public int Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public int QtyTickets
        {
            get { return _qtyTickets; }
            set { _qtyTickets = value; }
        }
        public Show Show
        {
            get { return _show; }
            set { _show = value; }
        }
        // Constructor
        public Booking(int phone, string email, int qtyTickets, Show show)
        {
            _phone = phone;
            _email = email;
            _qtyTickets = qtyTickets;
            _show = show;
        }
        // Methods
        public override string ToString()
        {
            return $"{_phone}, {_email}, {_qtyTickets}, {_show}";
        }
    }
}
