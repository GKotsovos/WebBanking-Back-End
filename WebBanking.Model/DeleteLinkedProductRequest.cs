using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public class DeleteLinkedProductRequest
    {
        public string CardId { get; }
        public string ProductId { get; }
    }
}
