using System;

namespace GeekBurguer.UI.Contract
{
    public class Allergy
    {
        public Guid AllergyId { get; set; }
        public string Name { get; set; }
        public bool Ativo { get; set; }
    }
}