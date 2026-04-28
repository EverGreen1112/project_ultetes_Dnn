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
            List<ProductTypeViewModel> products = new List<ProductTypeViewModel>();
            IEnumerable<TypePropertyDTO> typeProperties;

            using (IDataContext ctx = DataContext.Instance())
            {
                // Kibővített SQL: lekérjük a p.bvin-t is!
                string sqlProducts = @"
                    SELECT 
                        CAST(p.ProductTypeId AS NVARCHAR(50)) AS ProductTypeId,
                        CAST(p.bvin AS NVARCHAR(50)) AS ProductBvin,
                        p.RewriteUrl AS UrlSlug, 
                        pt.ProductName, 
                        ptt.ProductTypeName,
                        ct.Name as CategoryName
                    FROM hcc_Product p
                    INNER JOIN hcc_ProductXCategory pxc ON p.bvin = pxc.ProductId
                    INNER JOIN hcc_CategoryTranslations ct ON pxc.CategoryId = ct.CategoryId
                    INNER JOIN hcc_ProductTranslations pt ON p.bvin = pt.ProductId
                    INNER JOIN hcc_ProductTypeTranslations ptt 
                        ON CAST(p.ProductTypeId AS NVARCHAR(50)) = CAST(ptt.ProductTypeId AS NVARCHAR(50))
                    WHERE 
                        ct.Name IN (N'Zöldség vetőmag', N'Virág vetőmag', N'Prémium vetőmagok', N'Hagymák, gumók')
                        AND p.ProductTypeId IS NOT NULL
                ";

                var rawData = ctx.ExecuteQuery<ProductRawDTO>(System.Data.CommandType.Text, sqlProducts);

                foreach (var item in rawData)
                {
                    string group = "";
                    string catName = item.CategoryName;

                    if (catName == "Zöldség vetőmag") group = "Zoldseg";
                    else if (catName == "Virág vetőmag" || catName == "Prémium vetőmagok") group = "Virag";
                    else if (catName == "Hagymák, gumók") group = "Hagyma";

                    products.Add(new ProductTypeViewModel {
                        ProductTypeId = item.ProductTypeId,
                        ProductBvin = item.ProductBvin,
                        UrlSlug = item.UrlSlug, // <--- ITT MENTJÜK EL A SZÉP LINKET!
                        ProductName = item.ProductName,
                        ProductTypeName = item.ProductTypeName,
                        CategoryGroup = group
                    });
                }

                // 2. Tulajdonságok lekérése (marad a régi)
                string sqlProps = @"
            SELECT CAST(pxp.ProductTypeBvin AS NVARCHAR(50)) as ProductTypeId, pp.PropertyName
            FROM hcc_ProductTypeXProductProperty pxp
            INNER JOIN hcc_ProductProperty pp ON pxp.PropertyId = pp.Id";
                typeProperties = ctx.ExecuteQuery<TypePropertyDTO>(System.Data.CommandType.Text, sqlProps).ToList();
            }

            var calculator = new CalendarCalculator();
            var monthColors = calculator.CalculateMonthColors(products, typeProperties);

            ViewBag.MonthColors = monthColors;
            return View(products);
        }
    }
}
