/*
' Copyright (c) 2026 fa.html
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/
using DotNetNuke.Data;
using System.Collections.Generic;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;
using Ultetes.Dnn.Project_Ultetes_Dnn.Components;
using Ultetes.Dnn.Project_Ultetes_Dnn.Models;

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {

        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            var userlist = UserController.GetUsers(PortalSettings.PortalId);
            var users = from user in userlist.Cast<UserInfo>().ToList()
                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

            ViewBag.Users = users;

            var item = (itemId == -1)
                 ? new Item { ModuleId = ModuleContext.ModuleId }
                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            IEnumerable<ProductTypeViewModel> products;
            IEnumerable<TypePropertyDTO> typeProperties;

            using (IDataContext ctx = DataContext.Instance())
            {
                // 1. Lekérdezzük a termékeket (Kiegészítve a ProductTypeId-val)
                string sqlProducts = @"
                    SELECT 
                        CAST(p.ProductTypeId AS NVARCHAR(50)) AS ProductTypeId,
                        pt.ProductName, 
                        ptt.ProductTypeName
                    FROM hcc_Product p
                    INNER JOIN hcc_ProductXCategory pxc ON p.bvin = pxc.ProductId
                    INNER JOIN hcc_CategoryTranslations ct ON pxc.CategoryId = ct.CategoryId
                    INNER JOIN hcc_ProductTranslations pt ON p.bvin = pt.ProductId
                    INNER JOIN hcc_ProductTypeTranslations ptt 
                        ON CAST(p.ProductTypeId AS NVARCHAR(50)) = CAST(ptt.ProductTypeId AS NVARCHAR(50))
                    WHERE 
                        ct.Name = N'Zöldség vetőmag' 
                        AND p.ProductTypeId IS NOT NULL
                    ORDER BY 
                        ptt.ProductTypeName ASC, 
                        pt.ProductName ASC
                ";
                products = ctx.ExecuteQuery<ProductTypeViewModel>(System.Data.CommandType.Text, sqlProducts).ToList();

                // 2. Lekérdezzük az összes terméktípushoz tartozó tulajdonságot
                string sqlProps = @"
                    SELECT 
                        CAST(pxp.ProductTypeBvin AS NVARCHAR(50)) as ProductTypeId,
                        pp.PropertyName
                    FROM hcc_ProductTypeXProductProperty pxp
                    INNER JOIN hcc_ProductProperty pp ON pxp.PropertyId = pp.Id
                ";
                typeProperties = ctx.ExecuteQuery<TypePropertyDTO>(System.Data.CommandType.Text, sqlProps).ToList();
            }

            // 3. Összeállítjuk a naptár-mátrixot (Dictionary: TípusNév -> 12 elemű tömb a hónapoknak)
            var monthColors = new System.Collections.Generic.Dictionary<string, int[]>();

            foreach (var p in products)
            {
                if (!monthColors.ContainsKey(p.ProductTypeName))
                {
                    int[] months = new int[12]; // Alapból minden hónap 0 (üres)

                    // Kiválasztjuk a csak ehhez a típushoz tartozó tulajdonságokat
                    var propsForThisType = typeProperties.Where(t => t.ProductTypeId == p.ProductTypeId).ToList();

                    foreach (var prop in propsForThisType)
                    {
                        // Felvágjuk a nevet pl: "vetes_01_januar" -> ["vetes", "01", "januar"]
                        var parts = prop.PropertyName.Split('_');
                        if (parts.Length >= 2 && int.TryParse(parts[1], out int monthIndex))
                        {
                            monthIndex -= 1; // Mivel a tömb 0-tól indul (0-11), levonunk egyet

                            // Bitművelettel hozzáadjuk a státuszt
                            if (parts[0] == "vetes") months[monthIndex] |= 1; // 1 = Vetés
                            else if (parts[0] == "aratas") months[monthIndex] |= 2; // 2 = Aratás
                            // Ha mindkettő lefut (1+2), akkor 3 lesz az értéke (Zöld)
                        }
                    }
                    monthColors.Add(p.ProductTypeName, months);
                }
            }

            // Átadjuk a ViewBag segítségével a színmátrixot a View-nak
            ViewBag.MonthColors = monthColors;

            return View(products);
        }
    }
}
