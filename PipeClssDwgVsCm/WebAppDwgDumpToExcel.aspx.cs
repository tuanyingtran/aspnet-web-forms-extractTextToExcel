using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace PipeClssDwgVsCm
{
    public partial class WebFormDemo1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Load the check list
            if (!IsPostBack)
            {
                string[] columnHeaders;
                //Enum index to display in checkbox list, 25 is ApiClass 1 but display to represent 3 api
                int[] targetColumnPos = new int[] { 1, 5, 6, 7, 16, 17, 18, 25 };
                columnHeaders = GetCheckboxlistArrayData(targetColumnPos);
                //cblFieldSelect.DataSource = Enum.GetNames(typeof(ColumnPos));
                cblFieldSelect.DataSource = columnHeaders;
                cblFieldSelect.DataBind();
                //set to check all at default
                checkAllChbxList();

                ddlFileList.DataSource = GetDropDownFileList();
                ddlFileList.DataBind();
                CurrentTime.Text = DateTime.Now.ToString("y");
            }

        }

    //bind to dropdownlist, all the text file names(*.txt) on server
        private string[] GetDropDownFileList()
        {
            string fileBaseOnServerPath = Server.MapPath("~/DataFile/");
            string[] fullPathNameArray = Directory.GetFiles(fileBaseOnServerPath,"*.txt",SearchOption.TopDirectoryOnly);
            int arrayLength = fullPathNameArray.Length;
            string[] dropDownList = new string[arrayLength];
            for(int i =0; i<arrayLength;i++)
            {
                int lastSlashIndex = fullPathNameArray[i].LastIndexOf('\\');
                dropDownList[i] = fullPathNameArray[i].Substring(lastSlashIndex + 1,
                    fullPathNameArray[i].Length - (lastSlashIndex+5));//+1 for slash +4 for extension
                //dropDownList[i] = tempStrArray[i].Substring(tempStrArray[i].Length-8);//Get last 8 char,eg ####.txt
            }
            return dropDownList;
        }

        // Helper method for getting column headers: [1,5,6,7,16,17,18,25] for LocationID,...
        private string[] GetCheckboxlistArrayData(int[] pos)
        {
            int arrayLength = pos.Length;
            string[] columnHeaders = new string[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                columnHeaders[i] = Enum.GetName(typeof(ColumnPos), pos[i]);
            }
            return columnHeaders;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error for click event export to Excel
            // Confirms that an HtmlForm control is rendered for the
            //specified ASP.NET server control at run time.
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string addHeader2NdArg = "attachment;filename=" + tbFileName.Text + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", addHeader2NdArg);
            Response.Charset = "";
            // If you want the option to open the Excel file without saving than
            // comment out the line below
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            dataGridView1.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }


        protected void btnGetData_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> columnIndexSet = new Dictionary<string, int>();
            string fileName = "";//should be changed on user's input file
            
            string selectedDdl = ddlFileList.SelectedValue;//Selected value from dropdownlist
            string filenameToExcel = selectedDdl;
            if (selectedDdl.IsEmpty())
            {
                //TODO return alert
                
            }
            fileName = Server.MapPath("~/DataFile/")+selectedDdl+".txt";

            /*
            if (FileUpload1.HasFile)
            {
                //only file at related to server folder. Maybe to do saveas to sever when upload
                string fileBaseOnServerPath = Server.MapPath("~/DataFile/");
                //fileName = FileUpload1.PostedFile.FileName;
                //fileName = HttpContext.Current.Request.PhysicalApplicationPath + FileUpload1.FileName;
                filenameToExcel = FileUpload1.FileName;//to use this file name + .xls
                fileName = fileBaseOnServerPath + FileUpload1.FileName;

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page),
                    "ClientScript", "alert('Please choose file!')", true);

            }
            */

            //else ask user to browse to file to get data
            columnIndexSet = GetCheckboxList();
            DataTable dataTable = new DataTable();

            //Check if Api pipe class is selected then set flag if checked
            foreach (var col in columnIndexSet)
            {
                dataTable.Columns.Add(col.Key);
            }
            try
            {
                char[] splitCharArray = { '\t' };
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName))
                //using (System.IO.StreamReader sr = new System.IO.StreamReader("TuanData/DwgVsCm/0102.txt"))
                {
                    int whitespaceCol = 2;//Enum Position * 2 to actual Column in text file
                    while (sr.Peek() >= 0)
                    {
                        string[] readLineToArray = sr.ReadLine().Split(splitCharArray);
                        DataRow row = dataTable.NewRow();
                        foreach (var col in columnIndexSet)
                        {
                            if (col.Key != "ApiPipeClass")
                            {
                                row[col.Key] = readLineToArray[col.Value * whitespaceCol];//to including 'whitespace' column
                            }
                            else if (col.Key == "ApiPipeClass")
                            {
                                if (readLineToArray[col.Value * 2].Equals("X")) row[col.Key] = "1";//at 50
                                else if (readLineToArray[(col.Value * 2)+ 2].Equals("X")) row[col.Key] = "2";//at 52
                                else if (readLineToArray[(col.Value * 2) + 4].Equals("X")) row[col.Key] = "3";// at 54
                            }
                        }
                        dataTable.Rows.Add(row);//add row
                    }//while

                }//using
            }
            catch (Exception ex)
            {
                //Response.Write(@"<script language='javascript'>alert(${error});</script>");
                Response.Write(ex.Message);
            }
            dataGridView1.DataSource = dataTable;
            dataGridView1.DataBind();
            //filname, no extenstion, to save to xlx
            tbFileName.Text = filenameToExcel.Replace(".txt", "");
            btnExportToExcel.Enabled = true;
        }//btnGetData_Click

        //Return Dictionary in string(name), int (column index then * 2 to get actual index
        //Tranverse through all checkbox, then add to Dictionary collection
        ///public List<string> GetCheckboxList()
        public Dictionary<string, int> GetCheckboxList()
        {
            Dictionary<string, int> checkboxListSelected = new Dictionary<string, int>();
            foreach (ListItem item in cblFieldSelect.Items)
            {
                if (item.Selected)
                {
                    //add to dictionary: Column(string) , index(int)
                    //using global or in return
                    checkboxListSelected.Add(item.ToString(), (int)Enum.Parse(typeof(ColumnPos), item.ToString()));
                }
            }
            return checkboxListSelected;
        }//end GetCheckboxList

        //Check all items in checkbox list
        protected void btnCheckAll_Click(object sender, EventArgs e)
        {
            checkAllChbxList();
        }
        private void checkAllChbxList()
        {
            foreach (ListItem li in cblFieldSelect.Items)
            {
                li.Selected = true;
            }

        }
        protected void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in cblFieldSelect.Items)
            {
                li.Selected = false;
            }
        }

        // In column position from title block txt dump
        enum ColumnPos
        {
            //Column index without Blanks. To include blanks column => *2
            // 1,5,7,16,17,18,25 (represent 1 of 3)
            DwgFileName,
            LocationID,// 1st Posistion * 2 = actual column
            Revision,
            SheetNo,
            DwgDescription,
            System,// 5th Position
            Plant,// 6th Position
            Division,// 7th
            LastSaveDate,
            LastEditDftrCai,
            PlantDup,
            DisciplineCode,
            FileName,
            LineName,
            From,
            To,
            Material,// 16th
            PipeClass,// 17th
            Service,// 18th
            Pwht,
            _150RatingHeader,
            _150RatingCheckbox,
            _300RatingHeader,
            _300RatingCheckbox,
            StmTraceCheckbox,
            ApiPipeClass,// 25th. Any mark 'X' will be the class
            ApiPipeClass2,
            ApiPipeClass3,// 27th
            InsulatedCheckbox,
            Iso570Complete,
            Scale,
            DftrCreated,// 31st

            /*
           LocationID,
           RefineryPipeClass,
           PlantUnit,
           ApiPipeClass,
           System
           DwgFileName*/

        }//Enum

    }
}
