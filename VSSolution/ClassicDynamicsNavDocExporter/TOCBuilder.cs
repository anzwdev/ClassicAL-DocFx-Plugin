using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Dynamics.Nav.MetaModel;
using Microsoft.Dynamics.Nav.MetaMetaModel;
using Microsoft.Dynamics.Nav.Model.IO.Txt;
using ClassicDynamicsNavReference;
using ClassicDynamicsNavDocExporter.Models;

namespace ClassicDynamicsNavDocExporter
{
    public class TOCBuilder
    {

        public string ProjectUid { get; set; }
        public string ProjectCaption { get; set; }

        //toc objects
        public List<TocViewModel> TOCStructure { get; set; }
        protected Dictionary<string, TocViewModel> tocObjectTypesLevel;
        protected TocViewModel TOCRoot { get; set; }

        //grouping objects
        protected PageViewModel ProjectPage;
        protected List<PageViewModel> ObjectTypePages;

        protected Dictionary<string, ItemViewModel> objectCache;

        public TOCBuilder(string projectUid, string projectCaption)
        {
            objectCache = new Dictionary<string, ItemViewModel>();
            this.ProjectUid = projectUid;
            this.ProjectCaption = projectCaption;
            CreateTOC();
        }

        public void CreateTOC()
        {
            //create TOC structure
            this.TOCStructure = new List<TocViewModel>();
            //create TOC root
            TOCRoot = new TocViewModel();
            this.TOCStructure.Add(TOCRoot);
            TOCRoot.Uid = ProjectUid;
            TOCRoot.Name = ProjectCaption;
            TOCRoot.Items = new List<TocViewModel>();
            //create TOC object type level cache
            tocObjectTypesLevel = new Dictionary<string, TocViewModel>();
        }

        public void AddObject(ItemViewModel obj)
        {
            //try to find object type TOC entry
            TocViewModel tocTypeEntry = null;
            string objectTypeUid = UidHelpers.GetObjectTypeGroupUid(ProjectUid, obj.Type);
            if (this.tocObjectTypesLevel.ContainsKey(objectTypeUid))
                tocTypeEntry = this.tocObjectTypesLevel[objectTypeUid];
            else
            {
                tocTypeEntry = new TocViewModel();
                tocTypeEntry.Uid = objectTypeUid;
                tocTypeEntry.Name = obj.Type + "s";
                tocTypeEntry.Items = new List<TocViewModel>();
                TOCRoot.Items.Add(tocTypeEntry);
                tocObjectTypesLevel.Add(tocTypeEntry.Uid, tocTypeEntry);
            }

            TocViewModel tocDocElement = new TocViewModel();
            tocDocElement.Uid = obj.Uid;
            tocDocElement.Name = obj.Name;
            tocTypeEntry.Items.Add(tocDocElement);

            objectCache.Add(obj.Uid, obj);
        }

        public void ExportFiles(string path)
        {
            //export TOC
            string fileName = Path.Combine(path, "toc.yml");
            YamlHelper.ExportDocument(fileName, "TableOfContent", this.TOCStructure);

            //build group pages
            BuildGroupPages();

            //export group pages
            YamlHelper.ExportDocument(Path.Combine(path, this.ProjectUid + ".yml"), "ClassicDynamicsNavReference", this.ProjectPage);

            foreach (PageViewModel objTypePage in this.ObjectTypePages)
            {
                YamlHelper.ExportDocument(Path.Combine(path, objTypePage.Items[0].Uid + ".yml"), "ClassicDynamicsNavReference", objTypePage);
            }
        }

        public void BuildGroupPages()
        {
            //export data files
            ProjectPage = new PageViewModel();
            ObjectTypePages = new List<PageViewModel>();

            ItemViewModel mainItem = new ItemViewModel();
            mainItem.Uid = ProjectUid;
            mainItem.Name = ProjectCaption;
            mainItem.ItemType = ItemType.Project;
            mainItem.Items = new List<ItemViewModel>();

            ProjectPage.Items = new List<ItemViewModel>();
            ProjectPage.Items.Add(mainItem);

            foreach (TocViewModel tocObjectType in tocObjectTypesLevel.Values)
            {
                ItemViewModel objType = BuildObjectTypePage(tocObjectType);
                mainItem.Items.Add(objType);

                PageViewModel objTypePage = new PageViewModel();
                objTypePage.Items = new List<ItemViewModel>();
                objTypePage.Items.Add(objType);

                ObjectTypePages.Add(objTypePage);
            }

        }

        protected ItemViewModel BuildObjectTypePage(TocViewModel tocObjectType)
        {
            //build object type item
            ItemViewModel objectTypeItem = new ItemViewModel();
            objectTypeItem.Uid = tocObjectType.Uid;
            objectTypeItem.Name = tocObjectType.Name;
            objectTypeItem.ItemType = ItemType.Group;
            objectTypeItem.Items = new List<ItemViewModel>();

            //add items
            foreach (TocViewModel tocObj in tocObjectType.Items)
            {
                ItemViewModel obj = new ItemViewModel();
                obj.Uid = tocObj.Uid;
                obj.Name = tocObj.Name;
                obj.ItemType = ItemType.Object;

                if (objectCache.ContainsKey(tocObj.Uid))
                {
                    ItemViewModel srcObj = objectCache[tocObj.Uid];
                    obj.ItemType = srcObj.ItemType;
                    obj.ObjectId = srcObj.ObjectId;
                    obj.Summary = srcObj.Summary;
                }

                objectTypeItem.Items.Add(obj);
            }

            return objectTypeItem;
        }


    }
}
