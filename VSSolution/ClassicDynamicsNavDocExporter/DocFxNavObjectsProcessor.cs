using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Microsoft.Dynamics.Nav.MetaModel;
using Microsoft.Dynamics.Nav.MetaMetaModel;
using Microsoft.Dynamics.Nav.Model.IO.Txt;
using ClassicDynamicsNavReference;
using ClassicDynamicsNavDocExporter.Models;

namespace ClassicDynamicsNavDocExporter
{
    public class DocFxNavObjectsProcessor : NavObjectsProcessor
    {

        private int noOfObjects = 0;
        private int noOfErrors = 0;
        private Dictionary<string, ObjectLinkCache> objectLinks;

        public string ProjectUid = "NavProject";
        public string ProjectCaption = "";
        public TOCBuilder TOCBuilder { get; set; }

        public DocFxNavObjectsProcessor()
        {
        }

        public void ExportDocumentation(string sourceFileName, string destFolderName, string projectCaption)
        {
            this.ProjectCaption = projectCaption;

            //create TOC document
            this.TOCBuilder = new TOCBuilder(this.ProjectUid, this.ProjectCaption);

            noOfObjects = 0;
            noOfErrors = 0;

            //load source code
            LoadFromTextFile(sourceFileName);

            //build UIDs cache
            objectLinks = new Dictionary<string, ObjectLinkCache>();
            for (int i = 0; i < AppObjects.Count; i++)
            {
                AddObjectToLinkCache(AppObjects[i]);
            }

            //process objects
            for (int i = 0; i < AppObjects.Count; i++)
            {
                ExportObjectDocumentation(destFolderName, AppObjects[i]);
                noOfObjects++;
            }

            //export TOC
            TOCBuilder.ExportFiles(destFolderName);

            //write summary data
            Console.WriteLine($"{noOfObjects} object(s) exported.");
            if (noOfErrors > 0)
                Console.WriteLine($"{noOfErrors} error(s).");
        }

        #region Object Link cache

        protected void AddObjectToLinkCache(ApplicationObject appObject)
        {
            IElement rootElement = appObject.RootElement;
            ObjectLinkCache objLink = new ObjectLinkCache();
            objLink.Type = appObject.ElementTypeInfo.Name;
            objLink.Id = objLink.Type + rootElement.Id.ToString();
            objLink.Uid = UidHelpers.GetObjectUid(this.ProjectUid, objLink.Type, objLink.Id);
            objLink.Name = rootElement.GetStringProperty(PropertyType.Name);
            objectLinks.Add(objLink.Id, objLink);
        }

        protected ObjectLinkCache GetDataTypeLink(string dataType, string subType)
        {
            if ((String.IsNullOrWhiteSpace(dataType)) || (String.IsNullOrWhiteSpace(subType)))
                return null;

            dataType = dataType.Trim();
            subType = subType.Trim();
            if (dataType == "Record")
                dataType = "Table";
            string dataTypeId = dataType + subType;
            if (objectLinks.ContainsKey(dataTypeId))
                return objectLinks[dataTypeId];
            return null;
        }

        #endregion

        #region Object metadata export

        protected void ExportObjectDocumentation(string destFolderName, ApplicationObject appObject)
        {
            IElement rootElement = appObject.RootElement;
            IEnumerable<IElement> codeElementList = rootElement.GetSubElements(ElementType.Procedure);

            string objectName = rootElement.GetStringProperty(PropertyType.Name);
            string objectType = appObject.ElementTypeInfo.Name;
            int objectId = rootElement.Id;

            PageViewModel doc = new PageViewModel();
            doc.Items = new List<ItemViewModel>();

            ItemViewModel obj = new ItemViewModel();
            obj.Id = objectType + objectId.ToString();
            obj.Uid = UidHelpers.GetObjectUid(this.ProjectUid, objectType, obj.Id);
            obj.Name = objectName;
            obj.Type = objectType;
            obj.ObjectId = objectId;
            obj.ItemType = ItemType.Object;
            doc.Items.Add(obj);

            //write object documentation


            //write table fields and keys
            if (objectType == "Table")
            {
                obj.Fields = new List<FieldViewModel>();
                IEnumerable<IElement> fieldList = rootElement.GetSubElements(ElementType.Field);
                foreach (IElement fieldElement in fieldList)
                {
                    AddFieldDocumentation(doc, obj, fieldElement);
                }

                obj.Keys = new List<KeyViewModel>();
                IEnumerable<IElement> keyList = rootElement.GetSubElements(ElementType.Key);
                bool primaryKey = true;
                foreach (IElement keyElement in keyList)
                {
                    AddKeyDocumentation(doc, obj, keyElement, primaryKey);
                    primaryKey = false;
                }
            }

            if (objectType == "Page")
            {
                string sourceTable = rootElement.GetStringProperty(PropertyType.SourceTable);
                if (!String.IsNullOrWhiteSpace(sourceTable))
                {
                    sourceTable = sourceTable.Replace("Table", "");
                    obj.SourceTableId = sourceTable;

                    ObjectLinkCache sourceTableLink = GetDataTypeLink("Table", sourceTable);
                    if (sourceTableLink != null)
                    {
                        obj.SourceTableName = sourceTableLink.Name;
                        obj.SourceTableUid = sourceTableLink.Uid;
                    };

                    if (String.IsNullOrEmpty(obj.SourceTableName))
                        obj.SourceTableName = "Table " + sourceTable;
                }


                obj.Fields = new List<FieldViewModel>();
                IEnumerable<IElement> fieldList = rootElement.GetSubElements(ElementType.Control);
                foreach (IElement fieldElement in fieldList)
                {
                    AddPageFieldDocumentation(doc, obj, fieldElement);
                }
            }

            //write procedures
            foreach (IElement element in codeElementList)
            {
                AddProcedureDocumentation(doc, obj, element);
            }

            //export document
            YamlHelper.ExportDocument(Path.Combine(destFolderName, obj.Uid + ".yml"), "ClassicDynamicsNavReference", doc);
            
            //add object to TOC
            TOCBuilder.AddObject(obj);
        }

        #endregion

        protected void AddPageFieldDocumentation(PageViewModel doc, ItemViewModel obj, IElement fieldElement)
        {
            string source = fieldElement.GetStringProperty(PropertyType.SourceExpression);
            if (String.IsNullOrWhiteSpace(source))
                return;

            FieldViewModel fieldObj = new FieldViewModel();
            fieldObj.Id = obj.Uid + ".Field" + fieldElement.Id.ToString();
            fieldObj.Name = source;
            fieldObj.Summary = fieldElement.GetStringProperty(PropertyType.Description);

            obj.Fields.Add(fieldObj);
        }

        protected void AddFieldDocumentation(PageViewModel doc, ItemViewModel obj, IElement fieldElement)
        {
            FieldViewModel fieldObj = new FieldViewModel();
            fieldObj.Id = obj.Uid + ".Field" + fieldElement.Id.ToString();
            fieldObj.Name = fieldElement.Name;
            fieldObj.DataType = fieldElement.GetStringProperty(PropertyType.DataType);
            fieldObj.FieldId = fieldElement.Id.ToString();
            fieldObj.FieldClass = fieldElement.GetStringProperty(PropertyType.FieldClass);
            fieldObj.TableRelation = fieldElement.GetStringProperty(PropertyType.TableRelation);
            fieldObj.Formula = fieldElement.GetStringProperty(PropertyType.CalcFormula);
            fieldObj.Summary = fieldElement.GetStringProperty(PropertyType.Description);

            obj.Fields.Add(fieldObj);
        }

        protected void AddKeyDocumentation(PageViewModel doc, ItemViewModel obj, IElement keyElement, bool primaryKey)
        {
            KeyViewModel keyModel = new KeyViewModel();

            keyModel.PrimaryKey = primaryKey;
            keyModel.Fields = keyElement.GetStringProperty(PropertyType.Key);
            obj.Keys.Add(keyModel);
        }

        protected void AddProcedureDocumentation(PageViewModel doc, ItemViewModel obj, IElement procedureElement)
        {
            XmlDocument xmlDocumentation = null;
            try
            {
                StringBuilder xmlDocBuilder = new StringBuilder();
                xmlDocBuilder.AppendLine("<procedure>");
                GetXmlDocumentation(xmlDocBuilder, procedureElement);
                xmlDocBuilder.AppendLine("</procedure>");
                string xmlDocumentationText = xmlDocBuilder.ToString();
                xmlDocumentation = new XmlDocument();
                xmlDocumentation.LoadXml(xmlDocumentationText);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error processing xml documentation tags for procedure {procedureElement.Name} in object {procedureElement.Parent.Name}:");
                Console.WriteLine(e.Message);
                xmlDocumentation = new XmlDocument();
                xmlDocumentation.LoadXml("<procedure></procedure>");
                noOfErrors++;
            }

            //create procedure view model
            ItemViewModel procObj = new ItemViewModel();
            procObj.ItemType = ItemType.Procedure;
            procObj.Id = procedureElement.Name;
            procObj.Uid = obj.Uid + "." + procObj.Id;
            procObj.Name = procedureElement.Name;
            procObj.Syntax = new SyntaxDetailViewModel();

            //procedure visibility
            string local = procedureElement.GetStringProperty(PropertyType.Local);
            if ((local != null) && (local.ToLower() == "yes"))
                procObj.Syntax.Visibility = Visibility.Local;
            else
                procObj.Syntax.Visibility = Visibility.Public;

            //procedure type
            procObj.Syntax.ProcedureType = ProcedureType.Procedure;
            string attributes = procedureElement.GetStringProperty(PropertyType.Attributes);
            if (!String.IsNullOrWhiteSpace(attributes))
            {
                string attText = attributes.Trim().ToLower();
                if (attText.StartsWith("[eventsubscriber"))
                    procObj.Syntax.ProcedureType = ProcedureType.EventSubscriber;
                else if (attText.StartsWith("[business]"))
                {
                    procObj.Syntax.ProcedureType = ProcedureType.EventPublisher;
                    procObj.Syntax.EventPublisherType = EventPublisherType.Business;
                }
                else if (attText.StartsWith("[integration]"))
                {
                    procObj.Syntax.ProcedureType = ProcedureType.EventPublisher;
                    procObj.Syntax.EventPublisherType = EventPublisherType.Integration;
                }
            }

            //add to list of procedures, event publishers or event subscribers
            switch (procObj.Syntax.ProcedureType)
            {
                case ProcedureType.EventPublisher:
                    if (obj.EventPublishers == null)
                        obj.EventPublishers = new List<ItemViewModel>();
                    obj.EventPublishers.Add(procObj);
                    break;
                case ProcedureType.EventSubscriber:
                    if (obj.EventSubscribers == null)
                        obj.EventSubscribers = new List<ItemViewModel>();
                    obj.EventSubscribers.Add(procObj);
                    break;
                default:
                    if (obj.Procedures == null)
                        obj.Procedures = new List<ItemViewModel>();
                    obj.Procedures.Add(procObj);
                    break;
            }

            //get procedure documentation
            XmlElement procedureXmlElement = (XmlElement)xmlDocumentation.SelectSingleNode("/procedure");
            XmlNode summary = procedureXmlElement.SelectSingleNode("summary");
            if (summary != null)
                procObj.Summary = summary.InnerText;

            //collect xml documentation parameters and check them
            Dictionary<string, XmlElement> xmlDocParamatersDict = new Dictionary<string, XmlElement>();
            XmlNodeList xmlDocParamNodeList = procedureXmlElement.SelectNodes("param");
            if (xmlDocParamNodeList != null)
            {
                foreach (XmlElement xmlDocParamNode in xmlDocParamNodeList)
                {
                    XmlAttribute nameAttribute = xmlDocParamNode.Attributes["name"];
                    string name = null;
                    if (nameAttribute != null)
                        name = nameAttribute.Value;

                    if (String.IsNullOrWhiteSpace(name))
                    {
                        //delete attribute 
                        Console.Write($"Parameter without a name found in xml documentation for procedure {procedureElement.Name}");
                    }
                    else
                    {
                        if (xmlDocParamatersDict.ContainsKey(name))
                            Console.Write($"Duplicate parameter entry with name {name} has been found in xml documentation for procedure {procedureElement.Name}");
                        else
                            xmlDocParamatersDict.Add(name, xmlDocParamNode);
                    }
                }
            }

            //add parameters to the documentation
            int parameterIndex = 0;
            IEnumerable<IElement> parameterElementList = procedureElement.GetSubElements(Microsoft.Dynamics.Nav.MetaMetaModel.ElementType.Parameter);

            procObj.Syntax.Parameters = new List<ApiParameter>();

            //build procedure syntax string
            StringBuilder syntaxBuilder = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(attributes))
            {
                syntaxBuilder.Append(attributes);
                syntaxBuilder.Append("\n");
            }

            if (procObj.Syntax.Visibility == Visibility.Local)
                syntaxBuilder.Append("local ");
            syntaxBuilder.Append("procedure ");
            syntaxBuilder.Append(FormatHelpers.FormatName(procedureElement.Name));
            syntaxBuilder.Append("(");

            //process parameters
            foreach (IElement parameterElement in parameterElementList)
            {
                string parameterName = parameterElement.Name;

                ApiParameter paramObj = new ApiParameter();
                paramObj.Name = parameterName;
                paramObj.Type = parameterElement.GetStringProperty(PropertyType.DataType);
                paramObj.Length = parameterElement.GetStringProperty(PropertyType.Length);
                paramObj.ByVar = !String.IsNullOrWhiteSpace(parameterElement.GetStringProperty(PropertyType.AsVar));
                paramObj.SubType = parameterElement.GetStringProperty(PropertyType.Subtype);
                if (xmlDocParamatersDict.ContainsKey(parameterName))
                {
                    paramObj.Description = xmlDocParamatersDict[parameterName].InnerText;
                }
                //check if parameter is object and is part of this API
                ObjectLinkCache linkCache = GetDataTypeLink(paramObj.Type, paramObj.SubType);
                if (linkCache != null)
                {
                    paramObj.SubType = linkCache.Name;
                    paramObj.TypeUid = linkCache.Uid;
                }
                if (!String.IsNullOrWhiteSpace(paramObj.SubType))
                    paramObj.TypeFullName = paramObj.Type + " " + FormatHelpers.FormatName(paramObj.SubType);
                else
                    paramObj.TypeFullName = paramObj.Type;

                procObj.Syntax.Parameters.Add(paramObj);

                if (parameterIndex > 0)
                    syntaxBuilder.Append(", ");
                if (paramObj.ByVar)
                    syntaxBuilder.Append("VAR ");
                syntaxBuilder.Append(FormatHelpers.FormatName(paramObj.Name));
                syntaxBuilder.Append(": ");
                syntaxBuilder.Append(paramObj.TypeFullName);

                parameterIndex++;
            }

            syntaxBuilder.Append(")");

            //update return value
            IElement returnsElement = procedureElement.GetFirstSubElement(Microsoft.Dynamics.Nav.MetaMetaModel.ElementType.ReturnValue);
            if (returnsElement != null)
            {
                procObj.Syntax.Return = new ApiParameter();
                procObj.Syntax.Return.Name = returnsElement.Name;
                procObj.Syntax.Return.Type = returnsElement.GetStringProperty(PropertyType.DataType);
                procObj.Syntax.Return.Length = returnsElement.GetStringProperty(PropertyType.Length);
                procObj.Syntax.Return.TypeFullName = returnsElement.Name;

                XmlElement returnsXmlElement = (XmlElement)procedureXmlElement.SelectSingleNode("returns");
                if (returnsXmlElement != null)
                    procObj.Syntax.Return.Description = returnsXmlElement.InnerText;

                syntaxBuilder.Append(": ");
                syntaxBuilder.Append(procObj.Syntax.Return.Type);
            }

            //update procedure definition syntax
            procObj.Syntax.Content = syntaxBuilder.ToString();
        }



    }


}
