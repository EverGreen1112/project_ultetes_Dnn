using DotNetNuke.ComponentModel.DataAnnotations;
using System;

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Models
{
    // Megadjuk a pontos táblanevet
    [TableName("hcc_ProductType")]
    // A Hotcakes-ben a 'bvin' az elsődleges kulcs
    [PrimaryKey("bvin", AutoIncrement = false)]
    public class ProductType
    {
        public string bvin { get; set; }
    }
}