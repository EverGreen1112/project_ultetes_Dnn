using System;

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Models
{
    // Ezt az osztályt nem kell a DB-hez kötni [TableName]-el, 
    // mert csak egy egyedi SQL lekérdezés eredményét fogja tárolni.
    public class ProductTypeViewModel
    {
        public string ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeName { get; set; }
        public string CategoryGroup { get; set; }
    }
}  