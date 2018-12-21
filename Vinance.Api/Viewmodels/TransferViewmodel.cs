using System;

namespace Vinance.Api.Viewmodels
{
    public class TransferViewmodel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Amount { get; set; }

        public string Comment { get; set; }

        public int ToId { get; set; }

        public string ToName { get; set; }

        public int FromId { get; set; }

        public string FromName { get; set; }
    }
}