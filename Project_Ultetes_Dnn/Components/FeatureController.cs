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

//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using System.Collections.Generic;

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Components
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for Project_Ultetes_Dnn
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {


        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<Project_Ultetes_DnnInfo> colProject_Ultetes_Dnns = GetProject_Ultetes_Dnns(ModuleID);
        //if (colProject_Ultetes_Dnns.Count != 0)
        //{
        //    strXML += "<Project_Ultetes_Dnns>";

        //    foreach (Project_Ultetes_DnnInfo objProject_Ultetes_Dnn in colProject_Ultetes_Dnns)
        //    {
        //        strXML += "<Project_Ultetes_Dnn>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objProject_Ultetes_Dnn.Content) + "</content>";
        //        strXML += "</Project_Ultetes_Dnn>";
        //    }
        //    strXML += "</Project_Ultetes_Dnns>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlProject_Ultetes_Dnns = DotNetNuke.Common.Globals.GetContent(Content, "Project_Ultetes_Dnns");
        //foreach (XmlNode xmlProject_Ultetes_Dnn in xmlProject_Ultetes_Dnns.SelectNodes("Project_Ultetes_Dnn"))
        //{
        //    Project_Ultetes_DnnInfo objProject_Ultetes_Dnn = new Project_Ultetes_DnnInfo();
        //    objProject_Ultetes_Dnn.ModuleId = ModuleID;
        //    objProject_Ultetes_Dnn.Content = xmlProject_Ultetes_Dnn.SelectSingleNode("content").InnerText;
        //    objProject_Ultetes_Dnn.CreatedByUser = UserID;
        //    AddProject_Ultetes_Dnn(objProject_Ultetes_Dnn);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<Project_Ultetes_DnnInfo> colProject_Ultetes_Dnns = GetProject_Ultetes_Dnns(ModInfo.ModuleID);

        //foreach (Project_Ultetes_DnnInfo objProject_Ultetes_Dnn in colProject_Ultetes_Dnns)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objProject_Ultetes_Dnn.Content, objProject_Ultetes_Dnn.CreatedByUser, objProject_Ultetes_Dnn.CreatedDate, ModInfo.ModuleID, objProject_Ultetes_Dnn.ItemId.ToString(), objProject_Ultetes_Dnn.Content, "ItemId=" + objProject_Ultetes_Dnn.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }

}
