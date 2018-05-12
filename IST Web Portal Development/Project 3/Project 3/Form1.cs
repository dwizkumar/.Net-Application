using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using RestUtility;

namespace Project_3
{
    public partial class Form1 : Form
    {
        //global objects
        About about;
        Degrees degrees;
        Minors minors;
        Employment employments;
        Researches researches;
        People people1;
        Resources resource;
        Footer footers;
        News news;
        Course courses;

        //get our Restful resources...
        REST rj = new REST("http://ist.rit.edu/api");
        //another one we won't use...
        REST rx = new REST("http://google.com/api");

        //Initializes all the components
        public Form1()
        {
            InitializeComponent();
            pic_box_spinner.Visible = false;
        }

        //On Form Load, loads some of the resources such as about, resources, people
        private void Form1_Load(object sender, EventArgs e)
        {
            string jsonAbout = rj.getRestData("/about/");
            about = JToken.Parse(jsonAbout).ToObject<About>();
            //this.rtb_about.SelectionColor = Color.Red;   
            about_title.Text = "\n" + about.title;
            about_title.TextAlign = ContentAlignment.TopLeft;
            about_description.Text = "\n\n" + about.description;
            about_description.TextAlign = ContentAlignment.MiddleLeft;
            //about_description.
            about_quotes.Text = "\n\n\" " + about.quote + " \"";
            about_author.Text = "\n\n~" + about.quoteAuthor;

            string jsonResource = rj.getRestData("/resources/");
            resource = JToken.Parse(jsonResource).ToObject<Resources>();

            string jsonPeople = rj.getRestData("/people/");
            people1 = JToken.Parse(jsonPeople).ToObject<People>();
        }

        //Loads undergraduate and graduate degrees when enter into degree tab
        private void degree_Enter(object sender, EventArgs e)
        {
            //MessageBox.Show("hi");
            //load degree when it is null
            if (degrees == null)
            {
                string jsonDegrees = rj.getRestData("/degrees/");
                degrees = JToken.Parse(jsonDegrees).ToObject<Degrees>();
            }
            //clears the undergrads flow layout
            fl_out_undergrads.Controls.Clear();
            //loop through each of the undergrads array
            foreach (Undergraduate thisUndergrads in degrees.undergraduate)
            {
                Button btn = new Button();
                fl_out_undergrads.Padding = new Padding(30, 80, 0, 0);
                btn.Name = "btn_" + thisUndergrads.degreeName;
                btn.Text += thisUndergrads.title;
                btn.Text += "\n\n" + thisUndergrads.description;
                btn.Visible = true;
                btn.Height = 200;
                btn.Width = 230;
                btn.BackColor = Color.BlueViolet;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft Sans Serif", 16);
                fl_out_undergrads.Controls.Add(btn);
                var frm = new Form();
                frm.Text = thisUndergrads.title;
                frm.Width = 410;
                frm.Height = 340;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.Size = new Size(300, 250);
                Label lbl1 = new Label();
                lbl1.AutoSize = true;
                lbl1.MaximumSize = new Size(400,100);
                //on button click generates a form
                btn.Click += (sender1, args) =>
                {
                    string str = "";
                     lbl1.Text = thisUndergrads.title + "\n\n";
                    lbl1.ForeColor = Color.Red;
                    lbl1.Padding = new Padding(40,40,0,0);
                    lbl1.Font = new Font("Microsoft Sans Serif", 14);
                    frm.Controls.Add(lbl1);
                    foreach (string st in thisUndergrads.concentrations)
                        str += "\n" + st;
                    lbl.Text = str;
                    lbl.Font = new Font("Microsoft Sans Serif", 12);
                    lbl.Left = 60;
                    lbl.Top = 80;
                    frm.Controls.Add(lbl);
                    frm.ShowDialog();
                };
            }

            //clears the graduate flow layout
            fl_out_grads.Controls.Clear();
            fl_out_grads.Padding = new Padding(30, 20, 0, 0);
            //loop through each of the graduate degree
            foreach (Graduate thisGrads in degrees.graduate)
            {
                Button btn = new Button();
                if (thisGrads.degreeName == "graduate advanced certificates")
                {
                    btn.Name = "btn_" + thisGrads.degreeName;
                    btn.Text += thisGrads.degreeName;
                }
                else
                {
                    btn.Name = "btn_" + thisGrads.degreeName;
                    btn.Text += thisGrads.title;
                    btn.Text += "\n\n" + thisGrads.description;
                }
                btn.Visible = true;
                btn.Height = 200;
                btn.Width = 230;
                btn.BackColor = Color.BlueViolet;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft Sans Serif", 16);
                fl_out_grads.Controls.Add(btn);
                var frm = new Form();
                frm.Text = thisGrads.title;
                frm.Width = 410;
                frm.Height = 340;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.Size = new Size(300, 250);
                Label lbl1 = new Label();
                lbl1.AutoSize = true;
                lbl1.MaximumSize = new Size(400, 100);
                if (thisGrads.degreeName == "graduate advanced certificates")
                {   
                    //on button click generates the form
                    btn.Click += (sender1, args) =>
                    {
                        string str = "";
                        lbl1.Text = thisGrads.degreeName + "\n\n";
                        lbl1.ForeColor = Color.Red;
                        lbl1.Padding = new Padding(60, 40, 0, 0);
                        lbl1.Font = new Font("Microsoft Sans Serif", 14);
                        frm.Controls.Add(lbl1);
                        foreach (string st in thisGrads.availableCertificates)
                            str += "\n" + st;
                        lbl.Text = str;
                        lbl.Font = new Font("Microsoft Sans Serif", 12);
                        lbl.Left = 60;
                        lbl.Top = 80;
                        frm.Text = thisGrads.degreeName;
                        frm.Controls.Add(lbl);
                        frm.ShowDialog();
                    };
                }
                else
                {
                    //on button click generate graduate concentration
                    btn.Click += (sender1, args) =>
                    {
                        string str = "";
                        lbl1.Text  = thisGrads.title + "\n\n";
                        lbl1.ForeColor = Color.Red;
                        lbl1.Padding = new Padding(35, 40, 0, 0);
                        lbl1.Font = new Font("Microsoft Sans Serif", 14);
                        frm.Controls.Add(lbl1);
                        foreach (string st in thisGrads.concentrations)
                            str += "\n" + st;
                        lbl.Text = str;
                        lbl.Font = new Font("Microsoft Sans Serif", 12);
                        lbl.Left = 60;
                        lbl.Top = 80;
                        frm.Controls.Add(lbl);
                        frm.ShowDialog();
                    };
                }
            }
        }

        //Loads minors detail when enter into minor's tab
        private void minor_Enter(object sender, EventArgs e)
        {   
            //Load only when minor resouce is not loaded before
            if (minors == null)
            {
                string jsonMinors = rj.getRestData("/minors/");
                minors = JToken.Parse(jsonMinors).ToObject<Minors>();
            }
            //clears the minor flow layout pannel
            fl_out_minors.Controls.Clear();
            //FlowLayoutPanel cour = new FlowLayoutPanel();
            //cour.Controls.Clear();
            //loop through each of the minor detail
            foreach (UgMinor thisugMinors in minors.UgMinors)
            {
                Button btn = new Button();
                fl_out_minors.Padding = new Padding(80, 70, 0, 0);
                btn.Name = "btn_" + thisugMinors.name;
                btn.Text = thisugMinors.title;
                btn.Visible = true;
                btn.Height = 180;
                btn.Width = 180;
                btn.BackColor = Color.BlueViolet;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft Sans Serif", 16);
                fl_out_minors.Controls.Add(btn);
                var frm = new Form();
                //frm.Controls.Clear();
                frm.Text = thisugMinors.title;
                frm.Width = 1000;
                frm.Height = 450;
                frm.AutoSize = true;
                //frm.MaximumSize = new Size(0,450);
                //frm.AutoScroll = true;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.Size = new Size(950, 400);
                Label lbl2 = new Label();
                lbl2.AutoSize = true;
                lbl2.MaximumSize = new Size(950, 200);
                //on click generates minor's form
                btn.Click += (sender1, args) =>
                {
                    string str = "";
                    lbl2.Text = thisugMinors.title + "\n\n";
                    lbl2.ForeColor = Color.Maroon;
                    lbl2.Padding = new Padding(250, 10, 0, 0);
                    lbl2.Font = new Font("Microsoft Sans Serif", 14);
                    frm.Controls.Add(lbl2);
                    str += thisugMinors.description + "\n";
                    FlowLayoutPanel cour = new FlowLayoutPanel();
                    cour.AutoSize = true;
                    cour.Top = 375;
                    cour.Padding = new Padding(130, 0 , 0, 0);
                    cour.Controls.Clear();       
                    //generates course buttons dinamically and displays it in a form
                    foreach (string st in thisugMinors.courses)
                    {
                        //MessageBox.Show(st);
                        Button courseBtn = new Button();
                        courseBtn.Name = "crs_btn_" + st;
                        //MessageBox.Show(courseBtn.Name);
                        courseBtn.Text += st;
                        courseBtn.Visible = true;
                        courseBtn.Height = 50;
                        courseBtn.Width = 50;
                        courseBtn.AutoSize = true;
                        courseBtn.BackColor = Color.BlueViolet;
                        courseBtn.ForeColor = Color.White;
                        courseBtn.Font = new Font("Microsoft Sans Serif", 10);
                        //MessageBox.Show(cour.Controls.Count.ToString());
                        cour.Controls.Add(courseBtn);
                        //frm.Controls.Add(cour);
                        var frm1 = new Form();
                        //frm1.Controls.Clear();
                        frm1.Text = st;
                        frm1.Width = 1000;
                        frm1.Height = 80;
                        frm1.AutoSize = true;
                        //frm.MaximumSize = new Size(0,450);
                        //frm.AutoScroll = true;
                        frm1.StartPosition = FormStartPosition.CenterParent;
                        frm1.BackColor = Color.LightGreen;
                        Label lbl1 = new Label();
                        lbl1.Size = new Size(950, 150);
                        Label lbl3 = new Label();
                        lbl3.AutoSize = true;
                        lbl3.MaximumSize = new Size(950, 100);
                        string str1 = " ";
                        //on click of each of the course button
                        courseBtn.Click += (sender2, args1) => {
                            frm1.Controls.Clear();
                            lbl1.Controls.Clear();
                            string jsonCourse = rj.getRestData("/course/courseID="+st);
                            courses = JToken.Parse(jsonCourse).ToObject<Course>();
                            lbl3.Text = Regex.Replace(courses.title,"amp;","") + "\n";
                            lbl3.ForeColor = Color.Red;
                            lbl3.Padding = new Padding(300, 20, 0, 0);
                            lbl3.Font = new Font("Microsoft Sans Serif", 14);
                            frm1.Controls.Add(lbl3);
                            str1 += "\n" + courses.description;
                            lbl1.Text = str1;
                            lbl1.Font = new Font("Microsoft Sans Serif", 12);
                            lbl1.Left = 5;
                            lbl1.Top = 60;
                            frm1.Controls.Add(lbl1);
                            frm1.ShowDialog();
                        };
                    }
                    frm.Controls.Add(cour);
                    //cour.Controls.Clear();
                    str += "\n\n" + thisugMinors.note + "\n\n";
                    lbl.Text = str;
                    lbl.Font = new Font("Microsoft Sans Serif", 12);
                    lbl.Left = 5;
                    lbl.Top = 60;
                    frm.Controls.Add(lbl);
                    frm.ShowDialog();
                };
            }
        }

        //Loads employement details such as where our Employment,co-operative educations,and careers
        private void emplyoment_Enter(object sender, EventArgs e)
        {
            //Load resources if employments is null
            if (employments == null)
            {
                string jsonEmployments = rj.getRestData("/employment/");
                employments = JToken.Parse(jsonEmployments).ToObject<Employment>();
            }
            //rtb_employment.Controls.Clear();
            emp_title.Text = employments.introduction.title + "\n\n";
            String str = "\n\n\n";
            foreach (Content thisCont in employments.introduction.content)
            {
                str += "\n" + thisCont.title + "\n";
                str += "\n" + thisCont.description + "\n";
            }
            //rtb_employment.Text = str;
            //displays the list of employer who visits the campus
            str += "\n" + employments.employers.title + "\n\n";
            foreach (String thisEmployers in employments.employers.employerNames)
            {
                str += thisEmployers + "   ";
            }

            //displays the career scope at RIT
            str += "\n\n" + employments.careers.title + "\n\n";
            foreach (String thisCareers in employments.careers.careerNames)
            {
                str += thisCareers + "    ";
            }
            rtb_employment.Text = str;

            //shows all the satistics
            foreach (Statistic thisSats in employments.degreeStatistics.statistics)
            {
                Button btn = new Button();
                flp_stats.Padding = new Padding(3, 0, 0, 0);
                btn.Name = "pan_" + thisSats.value;
                btn.Text += thisSats.value + "\n\n";
                btn.Text += thisSats.description;
                btn.Visible = true;
                btn.Height = 180;
                btn.Width = 195;
                btn.BackColor = Color.DarkOrange;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft Sans Serif", 12);
                flp_stats.Controls.Add(btn);
            }
        }

        //Loads the employement table once employment button is clicked
        private void btn_emp_Click(object sender, EventArgs e)
        {
            //loads the employment resources
            if (employments == null)
            {
                string jsonEmployments = rj.getRestData("/employment/");
                employments = JToken.Parse(jsonEmployments).ToObject<Employment>();
            }
            //populate the tables' rows
            for(int i = 0; i < employments.employmentTable.professionalEmploymentInformation.Count; i++)
            {
                empl_dataGridView.Rows.Add();
                empl_dataGridView.Rows[i].Cells[0].Value = employments.employmentTable.professionalEmploymentInformation[i].employer;
                empl_dataGridView.Rows[i].Cells[1].Value = employments.employmentTable.professionalEmploymentInformation[i].degree;
                empl_dataGridView.Rows[i].Cells[2].Value = employments.employmentTable.professionalEmploymentInformation[i].city;
                empl_dataGridView.Rows[i].Cells[3].Value = employments.employmentTable.professionalEmploymentInformation[i].title;
                empl_dataGridView.Rows[i].Cells[4].Value = employments.employmentTable.professionalEmploymentInformation[i].startDate;
            }

            //disable the employment button for performance optimization
            //no second time load is necessary
            btn_emp.Enabled = false;
        }

        //Loads the co-op table once employment button is clicked
        private void btn_coop_Click(object sender, EventArgs e)
        {   
            //Load if employment is null
            if (employments == null)
            {
                string jsonEmployments = rj.getRestData("/employment/");
                employments = JToken.Parse(jsonEmployments).ToObject<Employment>();
            }

            //clears the datagrid. Although it is not required since I am enabling-disabling buttons
            coop_dataGridView.Controls.Clear();
            //populate the co-op table
            for(int i=0;i < employments.coopTable.coopInformation.Count; i++)
            {
                coop_dataGridView.Rows.Add();
                coop_dataGridView.Rows[i].Cells[0].Value = employments.coopTable.coopInformation[i].employer;
                coop_dataGridView.Rows[i].Cells[1].Value = employments.coopTable.coopInformation[i].degree;
                coop_dataGridView.Rows[i].Cells[2].Value = employments.coopTable.coopInformation[i].city;
                coop_dataGridView.Rows[i].Cells[3].Value = employments.coopTable.coopInformation[i].term;
            }
            //disable co-op button to prevent user to load the same table again unnecessarily
            btn_coop.Enabled = false;
        }

        //Loads faculty: area of interest that shows IST's domains such as mobile, HCI, analytics.
        private void fac_areaOfInterest_Enter(object sender, EventArgs e)
        {
            //load the research if its empty
            if (researches == null)
            {
                string jsonResearches = rj.getRestData("/research/");
                researches = JToken.Parse(jsonResearches).ToObject<Researches>();
            }
            //clear the flow layout
            fl_out_aoi.Controls.Clear();
            //loop through each of interest area
            foreach (ByInterestArea thisInterestArea in researches.byInterestArea)
            {
                Button btn = new Button();
                fl_out_aoi.Padding = new Padding(80, 50, 0, 0);
                btn.Name = "btn_" + thisInterestArea.areaName;
                btn.Text += thisInterestArea.areaName;
                btn.Visible = true;
                btn.Height = 140;
                btn.Width = 140;
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(4, 4, btn.Width - 8, btn.Height - 8);
                btn.Region = new Region(path);
                btn.BackColor = Color.BlueViolet;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft Sans Serif", 12);
                this.Controls.Add(btn);
                fl_out_aoi.Controls.Add(btn);
                var frm = new Form();
                frm.AutoScroll = true;
                frm.Text = thisInterestArea.areaName;
                frm.Width = 1000;
                frm.Height = 400;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960,0);
                Label lbl1 = new Label();
                lbl1.AutoSize = true;
                lbl1.MaximumSize = new Size(950, 200);
                //on button click display domains descriptions
                btn.Click += (sender1, args) =>
                {
                    string str = "";
                    lbl1.Text = "Research By Domain Area:" + thisInterestArea.areaName + "\n";
                    lbl1.ForeColor = Color.Red;
                    lbl1.Padding = new Padding(300, 20, 0, 0);
                    lbl1.Font = new Font("Microsoft Sans Serif", 14);
                    frm.Controls.Add(lbl1);
                    foreach (string st in thisInterestArea.citations)
                        str += "\n\n" + st;
                    lbl.Text = str;
                    lbl.Font = new Font("Microsoft Sans Serif", 12);
                    lbl.Left = 5;
                    lbl.Top = 20;
                    frm.Controls.Add(lbl);
                    frm.ShowDialog();
                };
            }
      }

        //Loads faculty: by lookup, displays the citations of different Professors
        private void fac_lookupByFaculty_Enter(object sender, EventArgs e)
        {
            //loads if research is null
            if (researches == null)
            {
                string jsonResearches = rj.getRestData("/research/");
                researches = JToken.Parse(jsonResearches).ToObject<Researches>();
            }
            //clear faculty lookup flowlayout
            fl_byfaclookup.Controls.Clear();
            //loop through each of faculty
            foreach (ByFaculty thisbyFaculty in researches.byFaculty)
            {
                Button btn = new Button();
                fl_byfaclookup.Padding = new Padding(60, 50, 0, 0);
                btn.Name = "btn_" + thisbyFaculty.facultyName;
                btn.Text += thisbyFaculty.facultyName;
                btn.Visible = true;
                btn.Height = 80;
                btn.Width = 150;
                btn.BackColor = Color.BlueViolet;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft Sans Serif", 12);
                this.Controls.Add(btn);
                fl_byfaclookup.Controls.Add(btn);
                var frm = new Form();
                frm.AutoScroll = true;
                frm.Text = thisbyFaculty.facultyName + " : " + thisbyFaculty.username;
                frm.Width = 1000;
                frm.Height = 400;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960, 0);
                Label lbl1 = new Label();
                lbl1.AutoSize = true;
                lbl1.MaximumSize = new Size(950, 200);
                //on click each of faculty displays citations of faculty
                btn.Click += (sender1, args) =>
                {
                    string str = "";
                    lbl1.Text =  thisbyFaculty.facultyName + " : Citations\n";
                    lbl1.ForeColor = Color.Red;
                    lbl1.Padding = new Padding(350, 20, 0, 0);
                    lbl1.Font = new Font("Microsoft Sans Serif", 14);
                    frm.Controls.Add(lbl1);
                    foreach (string st in thisbyFaculty.citations)
                        str += "\n\n" + st;
                    lbl.Text = str;
                    lbl.Font = new Font("Microsoft Sans Serif", 12);
                    lbl.Left = 5;
                    lbl.Top = 20;
                    frm.Controls.Add(lbl);
                    frm.ShowDialog();
                };
            }

            }

        //On button click displays picture of faculty and on clicking picture shows contact information
        //of the faculty memeber
        private async void btn_fac_Click(object sender, EventArgs e)
        {  
            //task - another way...
            //threaded and ASYNC.
            pic_box_spinner.Visible = true;
            //load the people if null
            if (researches == null)
            {
                await Task.Run(() =>
                { 
                string jsonPeople = rj.getRestData("/people/");
                people1 = JToken.Parse(jsonPeople).ToObject<People>();
            });
            }

            //clear people flowlayout pannel
            fl_out_ppl.Controls.Clear();
            //loop through each faculty
            foreach (Faculty thisFaculty in people1.faculty) {
                PictureBox picbox = new PictureBox();
                fl_byfaclookup.Padding = new Padding(81, 50, 0, 0);
                picbox.Name = "pic_" + thisFaculty.name;
                picbox.Visible = true;
                picbox.Height = 110;
                picbox.Width = 110;
                this.Controls.Add(picbox);
                picbox.Load(thisFaculty.imagePath);
                picbox.SizeMode = PictureBoxSizeMode.StretchImage;
                fl_out_ppl.Controls.Add(picbox);
                var frm = new Form();
                frm.AutoScroll = true;
                frm.Text = thisFaculty.name;
                frm.Width = 350;
                frm.Height = 310;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960, 0);
                Label lbl1 = new Label();
                lbl1.AutoSize = true;
                lbl1.MaximumSize = new Size(400, 100);
                //on click of picturebox displays contact information of the faculty member
                picbox.Click += (sender1, args) =>
                {
                    string str = "";
                    lbl1.Text = thisFaculty.name + ", "+ thisFaculty.title+"\n\n";
                    lbl1.ForeColor = Color.Maroon;
                    lbl1.Padding = new Padding(30, 20, 0, 0);
                    lbl1.Font = new Font("Microsoft Sans Serif", 12);
                    frm.Controls.Add(lbl1);
                    LinkLabel lnk_lbl = new LinkLabel();
                    //LinkLabel me = sender as LinkLabel;
                    LinkLabel lnk_lbl1 = new LinkLabel();
                    //check if email is not null
                    if (thisFaculty.email != null && thisFaculty.email != "")
                    {
                        str += "\nEmail: ";
                        lnk_lbl1.LinkVisited = false;
                        lnk_lbl1.Location = new System.Drawing.Point(80, 80);
                        lnk_lbl1.AutoSize = true;
                        //lnk_lbl1.Size = new System.Drawing.Size(135, 17);
                        lnk_lbl1.Text = thisFaculty.email;
                        lnk_lbl1.Font = new Font("Microsoft Sans Serif", 11);
                        lnk_lbl1.Tag = "mailto:" + thisFaculty.email;
                        frm.Controls.Add(lnk_lbl1);
                        lnk_lbl1.LinkClicked += (sender2, args2) => {
                            System.Diagnostics.Process.Start(lnk_lbl1.Tag.ToString());
                            lnk_lbl1.LinkVisited = true;
                        };
                    }

                    //check if website is not null
                    if (thisFaculty.website != null && thisFaculty.website != "")
                    {    
                        str += "\nWebsite: ";
                        lnk_lbl.LinkVisited = false;
                        lnk_lbl.Location = new System.Drawing.Point(100, 100);
                        //lbl.AutoSize = true;
                        lnk_lbl.Size = new System.Drawing.Size(140, 18);
                        lnk_lbl.Text = thisFaculty.website;
                        lnk_lbl.Font = new Font("Microsoft Sans Serif", 11);
                        lnk_lbl.Tag = thisFaculty.website;
                        frm.Controls.Add(lnk_lbl);
                        lnk_lbl.LinkClicked += (sender2,args2) => { System.Diagnostics.Process.Start(lnk_lbl.Tag.ToString());
                        lnk_lbl.LinkVisited = true;
                        };
                    }
                    
                    //check if office is not null
                    if (thisFaculty.office != null && thisFaculty.office != "")
                    {
                        str += "\nOffice: " + thisFaculty.office;
                    }

                    //check if phone is not null
                    if (thisFaculty.phone != null && thisFaculty.phone != "")
                    {
                        str += "\nPhone: " + thisFaculty.phone;
                    }

                    //check if twitter link is not null
                    if (thisFaculty.twitter != null && thisFaculty.twitter != "")
                    {
                        str += "\nTwitter: " + thisFaculty.twitter;
                    }

                    //check if facebook link is not null
                    if (thisFaculty.facebook != null && thisFaculty.facebook != "")
                    {
                        str += "\nFacebook: " + thisFaculty.facebook;
                    }
                    lbl.Text = str;
                    lbl.Font = new Font("Microsoft Sans Serif", 12);
                    lbl.Left = 30;
                    lbl.Top = 60;
                    frm.Controls.Add(lbl);
                    frm.ShowDialog();
                };
            }
            pic_box_spinner.Visible = false;
        }

        //On button click displays picture of staff and on clicking picture shows contact information
        //of the staff memeber
        private async void btn_staff_Click(object sender, EventArgs e)
        {
            //task - another way...
            //threaded and ASYNC.
            pic_box_spinner.Visible = true;
            //load the people if null
            if (researches == null)
            {
                await Task.Run(() =>
                {
                    string jsonPeople = rj.getRestData("/people/");
                    people1 = JToken.Parse(jsonPeople).ToObject<People>();
                });
            }

            //clears  the people layout
            fl_out_ppl.Controls.Clear();
            //loop through each of staff
            foreach (Staff thisStaff in people1.staff)
            {
                PictureBox picbox = new PictureBox();
                fl_byfaclookup.Padding = new Padding(81, 50, 0, 0);
                picbox.Name = "pic_" + thisStaff.name;
                picbox.Visible = true;
                picbox.Height = 110;
                picbox.Width = 110;
                //picbox.BackColor = Color.BlueViolet;
                this.Controls.Add(picbox);
                picbox.Load(thisStaff.imagePath);
                picbox.SizeMode = PictureBoxSizeMode.StretchImage;
                fl_out_ppl.Controls.Add(picbox);
                var frm = new Form();
                frm.AutoScroll = true;
                frm.Text = thisStaff.name;
                frm.Width = 370;
                frm.Height = 310;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.BackColor = Color.FromArgb(255, 160, 122);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960, 0);
                Label lbl1 = new Label();
                lbl1.AutoSize = true;
                lbl1.MaximumSize = new Size(400, 100);
                //on click of each picbox shows contact details
                picbox.Click += (sender1, args) =>
                {
                    string str = "";
                    lbl1.Text = thisStaff.name + ", " + thisStaff.title + "\n\n";
                    lbl1.ForeColor = Color.Maroon;
                    lbl1.Padding = new Padding(30, 20, 0, 0);
                    lbl1.Font = new Font("Microsoft Sans Serif", 12);
                    frm.Controls.Add(lbl1);
                    LinkLabel lnk_lbl = new LinkLabel();
                    //LinkLabel me = sender as LinkLabel;
                    LinkLabel lnk_lbl1 = new LinkLabel();
                    if (thisStaff.email != null && thisStaff.email != "")
                    {
                        str += "\nEmail: ";
                        lnk_lbl1.LinkVisited = false;
                        lnk_lbl1.Location = new System.Drawing.Point(80, 80);
                        lnk_lbl1.AutoSize = true;
                        //lnk_lbl1.Size = new System.Drawing.Size(135, 17);
                        lnk_lbl1.Text = thisStaff.email;
                        lnk_lbl1.Font = new Font("Microsoft Sans Serif", 11);
                        lnk_lbl1.Tag = "mailto:" + thisStaff.email;
                        frm.Controls.Add(lnk_lbl1);
                        lnk_lbl1.LinkClicked += (sender2, args2) => {
                            System.Diagnostics.Process.Start(lnk_lbl1.Tag.ToString());
                            lnk_lbl1.LinkVisited = true;
                        };
                    }

                    //check each of the attributes and display if its not null
                    if (thisStaff.website != null && thisStaff.website != "")
                    {
                        str += "\nWebsite: ";
                        lnk_lbl.LinkVisited = false;
                        lnk_lbl.Location = new System.Drawing.Point(100, 100);
                        //lbl.AutoSize = true;
                        lnk_lbl.Size = new System.Drawing.Size(140, 18);
                        lnk_lbl.Text = thisStaff.website;
                        lnk_lbl.Font = new Font("Microsoft Sans Serif", 11);
                        lnk_lbl.Tag = thisStaff.website;
                        frm.Controls.Add(lnk_lbl);
                        lnk_lbl.LinkClicked += (sender2, args2) => {
                            System.Diagnostics.Process.Start(lnk_lbl.Tag.ToString());
                            lnk_lbl.LinkVisited = true;
                        };
                    }

                    if (thisStaff.office != null && thisStaff.office != "")
                    {
                        str += "\nOffice: " + thisStaff.office;
                    }
                    if (thisStaff.phone != null && thisStaff.phone != "")
                    {
                        str += "\nPhone: " + thisStaff.phone;
                    }
                    if (thisStaff.twitter != null && thisStaff.twitter != "")
                    {
                        str += "\nTwitter: " + thisStaff.twitter;
                    }
                    if (thisStaff.facebook != null && thisStaff.facebook != "")
                    {
                        str += "\nFacebook: " + thisStaff.facebook;
                    }
                    lbl.Text = str;
                    lbl.Font = new Font("Microsoft Sans Serif", 12);
                    lbl.Left = 30;
                    lbl.Top = 60;
                    frm.Controls.Add(lbl);
                    frm.ShowDialog();
                };
            }
            pic_box_spinner.Visible = false;
        }

        //Loads resources on enter the resource tab
        private void resources_Enter(object sender, EventArgs e)
        {
            //loads if resource tab is null
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }

            //displays only if advising pannel is clicked
            btn_acad_advisors.Visible = false;
            btn_prof_advisors.Visible = false;
            btn_fac_advisors.Visible = false;
            btn_minor_advising.Visible = false;

            //load abroad image
            pic_box_sdAbroad.Load("http://ist.rit.edu/assets/img/resources/current_study_abroad.jpg");
            pic_box_sdAbroad.SizeMode = PictureBoxSizeMode.StretchImage;
        
            Label lbl_sdAbroad = new Label();
            lbl_sdAbroad.Text = resource.studyAbroad.title;
            lbl_sdAbroad.AutoSize = true;
            lbl_sdAbroad.Font = new Font("Monotype Corsiva", 11);
            lbl_sdAbroad.BackColor = Color.DarkOrange;
            lbl_sdAbroad.Left = 40;
            lbl_sdAbroad.Top = 90;
            lbl_sdAbroad.Parent = pic_box_sdAbroad;

            //load form image
            pic_box_forms.Load("http://ist.rit.edu/assets/img/resources/current_forms.jpg");
            pic_box_forms.SizeMode = PictureBoxSizeMode.StretchImage;
            Label lbl_forms = new Label();
            lbl_forms.Text = "Forms";
            lbl_forms.AutoSize = true;
            lbl_forms.Font = new Font("Monotype Corsiva", 11);
            lbl_forms.BackColor = Color.DarkOrange;
            lbl_forms.Left = 60;
            lbl_forms.Top = 90;
            lbl_forms.Parent = pic_box_forms;

            //load co-op image
            pic_box_coopEnr.Load("http://ist.rit.edu/assets/img/resources/current_co-op.jpg");
            pic_box_coopEnr.SizeMode = PictureBoxSizeMode.StretchImage;
            Label lbl_coopEnr = new Label();
            lbl_coopEnr.Text = resource.coopEnrollment.title;
            lbl_coopEnr.AutoSize = true;
            lbl_coopEnr.Font = new Font("Monotype Corsiva", 11);
            lbl_coopEnr.BackColor = Color.DarkOrange;
            lbl_coopEnr.Left = 40;
            lbl_coopEnr.Top = 90;
            lbl_coopEnr.Parent = pic_box_coopEnr;

            //load lab image
            pic_box_labInfo.Load("http://ist.rit.edu/assets/img/resources/current_tutors_lab.jpg");
            pic_box_labInfo.SizeMode = PictureBoxSizeMode.StretchImage;
            Label lbl_labInfo = new Label();
            lbl_labInfo.Text = resource.tutorsAndLabInformation.title;
            lbl_labInfo.AutoSize = true;
            lbl_labInfo.Font = new Font("Monotype Corsiva", 11);
            lbl_labInfo.BackColor = Color.DarkOrange;
            lbl_labInfo.Left = 20;
            lbl_labInfo.Top = 90;
            lbl_labInfo.Parent = pic_box_labInfo;

            //load ambasador image
            pic_box_studAmb.Load("http://ist.rit.edu/assets/img/resources/current_student_amb.jpg");
            pic_box_studAmb.SizeMode = PictureBoxSizeMode.StretchImage;
            Label lbl_stud_amb = new Label();
            lbl_stud_amb.Text = "Student Ambassador";
            lbl_stud_amb.AutoSize = true;
            lbl_stud_amb.Font = new Font("Monotype Corsiva", 11);
            lbl_stud_amb.BackColor = Color.DarkOrange;
            lbl_stud_amb.Left = 30;
            lbl_stud_amb.Top = 90;
            lbl_stud_amb.Parent = pic_box_studAmb;

            //load advising image
            pic_box_advising.Load("http://ist.rit.edu/assets/img/resources/current_student_advising.jpg");
            pic_box_advising.SizeMode = PictureBoxSizeMode.StretchImage;
            Label lbl_stud_adv = new Label();
            lbl_stud_adv.Text = resource.studentServices.title;
            lbl_stud_adv.AutoSize = true;
            lbl_stud_adv.Font = new Font("Monotype Corsiva", 11);
            lbl_stud_adv.BackColor = Color.DarkOrange;
            lbl_stud_adv.Left = 50;
            lbl_stud_adv.Top = 90;
            lbl_stud_adv.Parent = pic_box_advising;

            //add titles for advising buttons
            btn_acad_advisors.Text = resource.studentServices.academicAdvisors.title;
            btn_prof_advisors.Text = resource.studentServices.professonalAdvisors.title;
            btn_fac_advisors.Text = resource.studentServices.facultyAdvisors.title;
            btn_minor_advising.Text = resource.studentServices.istMinorAdvising.title;
        }

        //On click show aborad details
        private void pic_box_sdAbroad_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }
            btn_acad_advisors.Visible = false;
            btn_prof_advisors.Visible = false;
            btn_fac_advisors.Visible = false;
            btn_minor_advising.Visible = false;

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.studyAbroad.title;
            frm.Width = 1000;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            Label lbl = new Label();
            lbl.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl.MaximumSize = new System.Drawing.Size(960, 0);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(960, 0);
            lbl.Font = new Font("Microsoft Sans Serif", 12);
            lbl.Left = 5;
            lbl.Top = 20;
            lbl1.Text = resource.studyAbroad.title;
            lbl1.ForeColor = Color.Maroon;
            lbl1.Padding = new Padding(400, 20, 0, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            frm.Controls.Add(lbl1);
            lbl.Text += "\n\n" + resource.studyAbroad.description;
            foreach(Place place in resource.studyAbroad.places)
            {
                lbl.Text += "\n\n" + place.nameOfPlace;
                lbl.Text += "\n\n" + place.description;
            }
            frm.Controls.Add(lbl);
            frm.ShowDialog();
        }

        //On click show various forms for undergrads and grads
        private void pic_box_forms_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }
            btn_acad_advisors.Visible = false;
            btn_prof_advisors.Visible = false;
            btn_fac_advisors.Visible = false;
            btn_minor_advising.Visible = false;

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = "Forms";
            frm.Width = 400;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            
            //lbl.Dock = DockStyle.Top;
            //frm.Controls.Add(lbl);

            foreach (GraduateForm gradForm in resource.forms.graduateForms)
            {
                LinkLabel lnk = new LinkLabel();
                lnk.Name = "lnk_" + gradForm.formName;
                lnk.LinkVisited = false;
                lnk.AutoSize = true;
                lnk.Text = gradForm.formName;
                lnk.Dock = DockStyle.Top;
                lnk.Font = new Font("Microsoft Sans Serif", 11);
                lnk.Tag = "https://ist.rit.edu/" + gradForm.href;
                frm.Controls.Add(lnk);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960, 0);
                lbl.Font = new Font("Microsoft Sans Serif", 12);
                lbl.Left = 20;
                lbl.Top = 20;
                lbl.Text += "\n" + gradForm.formName;
                lbl.Dock = DockStyle.Top;
                frm.Controls.Add(lbl);
                lnk.LinkClicked += (sender1, args) => { System.Diagnostics.Process.Start(lnk.Tag.ToString());
                    lnk.LinkVisited = true;
                };
            }
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl1.MaximumSize = new System.Drawing.Size(960, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            lbl1.ForeColor = Color.Maroon;
            lbl1.Text += "\n\n"+ "Graduate Forms";
            lbl1.Dock = DockStyle.Top;
            frm.Controls.Add(lbl1);

            LinkLabel lnk1 = new LinkLabel();
            lnk1.Name = "lnk_" + resource.forms.undergraduateForms[0].formName;
            lnk1.LinkVisited = false;
            lnk1.AutoSize = true;
            lnk1.Text = resource.forms.undergraduateForms[0].formName;
            lnk1.Dock = DockStyle.Top;
            lnk1.Font = new Font("Microsoft Sans Serif", 11);
            lnk1.Tag = "https://ist.rit.edu/" + resource.forms.undergraduateForms[0].href;
            frm.Controls.Add(lnk1);
            Label lbl2 = new Label();
            lbl2.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl2.MaximumSize = new System.Drawing.Size(960, 0);
            lbl2.Font = new Font("Microsoft Sans Serif", 12);
            lbl2.Left = 20;
            lbl2.Top = 20;
            lbl2.Text += "\n" + resource.forms.undergraduateForms[0].formName;
            lbl2.Dock = DockStyle.Top;
            frm.Controls.Add(lbl2);
            lnk1.LinkClicked += (sender1, args) => {
                System.Diagnostics.Process.Start(lnk1.Tag.ToString());
                lnk1.LinkVisited = true;
            };

            Label lbl3 = new Label();
            lbl3.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl3.MaximumSize = new System.Drawing.Size(960, 0);
            lbl3.Font = new Font("Microsoft Sans Serif", 14);
            lbl3.ForeColor = Color.Maroon;
            lbl3.Text = "Forms";
            lbl3.Text += "\n\n" + "UnderGraduate Forms";
            lbl3.Dock = DockStyle.Top;
            frm.Controls.Add(lbl3);

            frm.ShowDialog();
        }

        //On click show details of co-op enrollment
        private void pic_box_coopEnr_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }
            btn_acad_advisors.Visible = false;
            btn_prof_advisors.Visible = false;
            btn_fac_advisors.Visible = false;
            btn_minor_advising.Visible = false;

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.coopEnrollment.title;
            frm.Width = 1000;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            Label lbl = new Label();
            lbl.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl.MaximumSize = new System.Drawing.Size(960, 0);
            lbl.Font = new Font("Microsoft Sans Serif", 12);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(960, 0);
            lbl.Left = 5;
            lbl.Top = 20;
            lbl1.Text = resource.coopEnrollment.title;
            lbl1.ForeColor = Color.Maroon;
            lbl1.Padding = new Padding(400, 20, 0, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            frm.Controls.Add(lbl1);
            foreach (EnrollmentInformationContent coopEnr in resource.coopEnrollment.enrollmentInformationContent)
            {
                lbl.Text += "\n\n" + coopEnr.title;
                lbl.Text += "\n\n" + coopEnr.description;
            }
            lbl.Text += "\n\n" + "RITJobZoneGuidelink:" + "\n";
            frm.Controls.Add(lbl);
            LinkLabel lnk = new LinkLabel();
            lnk.Font = new Font("Microsoft Sans Serif", 12);
            lnk.Text = " " + resource.coopEnrollment.RITJobZoneGuidelink;
            lnk.Tag =  resource.coopEnrollment.RITJobZoneGuidelink;
            //lnk.Left = 15;
            lnk.Dock = DockStyle.Bottom;
            lnk.LinkClicked += (sender1, args) => {
                System.Diagnostics.Process.Start(lnk.Tag.ToString());
                lnk.LinkVisited = true;
            };
            frm.Controls.Add(lnk);
            frm.ShowDialog();
        }

        //On click show details about lab infromation
        private void pic_box_labInfo_Click(object sender, EventArgs e)
        {

            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }
            btn_acad_advisors.Visible = false;
            btn_prof_advisors.Visible = false;
            btn_fac_advisors.Visible = false;
            btn_minor_advising.Visible = false;

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.tutorsAndLabInformation.title;
            frm.Width = 1000;
            frm.Height = 200;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            Label lbl = new Label();
            lbl.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl.MaximumSize = new System.Drawing.Size(960, 0);
            lbl.Font = new Font("Microsoft Sans Serif", 12);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(960, 0);
            lbl.Left = 5;
            lbl.Top = 20;
            lbl1.Text = resource.tutorsAndLabInformation.title;
            lbl1.ForeColor = Color.Maroon;
            lbl1.Padding = new Padding(400, 20, 0, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            frm.Controls.Add(lbl1);
            lbl.Text += "\n\n" + resource.tutorsAndLabInformation.description;
            frm.Controls.Add(lbl);
            frm.ShowDialog();
        }

        //On click show details of student ambassador
        private void pic_box_studAmb_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }
            btn_acad_advisors.Visible = false;
            btn_prof_advisors.Visible = false;
            btn_fac_advisors.Visible = false;
            btn_minor_advising.Visible = false;

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.studentAmbassadors.title;
            frm.Width = 1000;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            Label lbl = new Label();
            lbl.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl.MaximumSize = new System.Drawing.Size(960, 0);
            lbl.Font = new Font("Microsoft Sans Serif", 12);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(960, 0);
            lbl.Left = 5;
            lbl.Top = 20;
            lbl1.Text = resource.studentAmbassadors.title;
            lbl1.ForeColor = Color.Maroon;
            lbl1.Padding = new Padding(250, 20, 0, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            frm.Controls.Add(lbl1);
            foreach (SubSectionContent subSec in resource.studentAmbassadors.subSectionContent)
            {
                lbl.Text += "\n\n" + subSec.title;
                lbl.Text += "\n\n" + subSec.description;
            }
            lbl.Text += "\n\n" + resource.studentAmbassadors.note;
            lbl.Text += "\n\n" + "Application From Link:";
            frm.Controls.Add(lbl);

            LinkLabel lnk = new LinkLabel();
            lnk.Font = new Font("Microsoft Sans Serif", 12);
            lnk.Text = " " + resource.studentAmbassadors.applicationFormLink;
            lnk.Tag = resource.studentAmbassadors.applicationFormLink;
            lnk.Dock = DockStyle.Bottom;
            lnk.LinkClicked += (sender1, args) => {
                System.Diagnostics.Process.Start(lnk.Tag.ToString());
                lnk.LinkVisited = true;
            };
            frm.Controls.Add(lnk);
            frm.ShowDialog();
        }

        //On click popluate 4 bouttons- academic advisors, professional advisors,
        //faculty advisors, and minor advising
        private void pic_box_advising_Click(object sender, EventArgs e)
        {
            btn_acad_advisors.Visible = true;
            btn_prof_advisors.Visible = true;
            btn_fac_advisors.Visible = true;
            btn_minor_advising.Visible = true;            
        }

        //Displays details of academic advisors
        private void btn_acad_advisors_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.studentServices.academicAdvisors.title;
            frm.Width = 1000;
            frm.Height = 200;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            Label lbl = new Label();
            lbl.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl.MaximumSize = new System.Drawing.Size(960, 0);
            lbl.Font = new Font("Microsoft Sans Serif", 12);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(960, 0);
            lbl.Left = 5;
            lbl.Top = 20;
            lbl1.Text = resource.studentServices.academicAdvisors.title;
            lbl1.ForeColor = Color.Maroon;
            lbl1.Padding = new Padding(400, 20, 0, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            frm.Controls.Add(lbl1);
            lbl.Text += "\n\n" + resource.studentServices.academicAdvisors.description;
            lbl.Text += "\n\n" + resource.studentServices.academicAdvisors.faq.title;
            frm.Controls.Add(lbl);

            LinkLabel lnk = new LinkLabel();
            lnk.Font = new Font("Microsoft Sans Serif", 12);
            lnk.Text = " " + resource.studentServices.academicAdvisors.faq.contentHref;
            lnk.Tag = resource.studentServices.academicAdvisors.faq.contentHref;
            lnk.Dock = DockStyle.Bottom;
            lnk.LinkClicked += (sender1, args) => {
                System.Diagnostics.Process.Start(lnk.Tag.ToString());
                lnk.LinkVisited = true;
            };
            frm.Controls.Add(lnk);
            frm.ShowDialog();
        }

        //Displays details of professional advisors
        private void btn_prof_advisors_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.studentServices.professonalAdvisors.title;
            frm.Width = 400;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);

            foreach (AdvisorInformation advInfo in resource.studentServices.professonalAdvisors.advisorInformation)
            {
                LinkLabel lnk = new LinkLabel();
                lnk.Name = "lnk_" + advInfo.email;
                lnk.LinkVisited = false;
                lnk.AutoSize = true;
                lnk.Dock = DockStyle.Top;
                lnk.Font = new Font("Microsoft Sans Serif", 11);
                lnk.Text = advInfo.email;
                lnk.Tag = "mailto:" + advInfo.email;
                frm.Controls.Add(lnk);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960, 0);
                lbl.Font = new Font("Microsoft Sans Serif", 12);
                lbl.Left = 20;
                lbl.Top = 20;
                lbl.Text += "\n\n" + advInfo.department;
                lbl.Text += "\n" + advInfo.name;
                lbl.Dock = DockStyle.Top;
                frm.Controls.Add(lbl);
                lnk.LinkClicked += (sender1, args) => {
                    System.Diagnostics.Process.Start(lnk.Tag.ToString());
                    lnk.LinkVisited = true;
                };
            }

            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl1.MaximumSize = new System.Drawing.Size(960, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            lbl1.ForeColor = Color.Maroon;
            lbl1.Text += resource.studentServices.professonalAdvisors.title;
            lbl1.Dock = DockStyle.Top;
            frm.Controls.Add(lbl1);
            frm.ShowDialog();
        }

        //Displays details of faculty advisors
        private void btn_fac_advisors_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.studentServices.facultyAdvisors.title;
            frm.Width = 1000;
            frm.Height = 300;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            Label lbl = new Label();
            lbl.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl.MaximumSize = new System.Drawing.Size(960, 0);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(960, 0);
            lbl.Font = new Font("Microsoft Sans Serif", 12);
            lbl.Left = 5;
            lbl.Top = 20;
            lbl1.Text = resource.studentServices.facultyAdvisors.title;
            lbl1.ForeColor = Color.Maroon;
            lbl1.Padding = new Padding(400, 20, 0, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            frm.Controls.Add(lbl1);
            lbl.Text += "\n\n" + resource.studentServices.facultyAdvisors.description;
            frm.Controls.Add(lbl);
            frm.ShowDialog();
        }

        //Displays results of minor advising
        private void btn_minor_advising_Click(object sender, EventArgs e)
        {
            if (resource == null)
            {
                string jsonResource = rj.getRestData("/resources/");
                resource = JToken.Parse(jsonResource).ToObject<Resources>();
            }

            var frm = new Form();
            frm.AutoScroll = true;
            frm.Text = resource.studentServices.istMinorAdvising.title;
            frm.Width = 400;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);

            foreach (MinorAdvisorInformation minAdvInfo in resource.studentServices.istMinorAdvising.minorAdvisorInformation)
            {
                LinkLabel lnk = new LinkLabel();
                lnk.Name = "lnk_" + minAdvInfo.email;
                lnk.LinkVisited = false;
                lnk.AutoSize = true;
                lnk.Dock = DockStyle.Top;
                lnk.Font = new Font("Microsoft Sans Serif", 11);
                lnk.Text = minAdvInfo.email;
                lnk.Tag = "mailto:" + minAdvInfo.email;
                frm.Controls.Add(lnk);
                Label lbl = new Label();
                lbl.AutoSize = true;
                //lbl.Size = new Size(0, 400);
                lbl.MaximumSize = new System.Drawing.Size(960, 0);
                lbl.Font = new Font("Microsoft Sans Serif", 12);
                lbl.Left = 20;
                lbl.Top = 20;
                lbl.Text += "\n" + minAdvInfo.advisor;
                lbl.Text += "\n" + minAdvInfo.title;
                lbl.Dock = DockStyle.Top;
                frm.Controls.Add(lbl);
                lnk.LinkClicked += (sender1, args) => {
                    System.Diagnostics.Process.Start(lnk.Tag.ToString());
                    lnk.LinkVisited = true;
                };
            }

            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            //lbl.Size = new Size(0, 400);
            lbl1.MaximumSize = new System.Drawing.Size(960, 0);
            lbl1.Font = new Font("Microsoft Sans Serif", 14);
            lbl1.ForeColor = Color.Maroon;
            lbl1.Text += resource.studentServices.istMinorAdvising.title;
            lbl1.Dock = DockStyle.Top;
            frm.Controls.Add(lbl1);
            frm.ShowDialog();
        }

        //On enter shows footer details such as apply now, about website, support IST, lab hours, and news
        private void footer_Enter(object sender, EventArgs e)
        {
            if (footers == null)
            {
                string jsonFooter = rj.getRestData("/footer/");
                footers = JToken.Parse(jsonFooter).ToObject<Footer>();
            }

            lbl_footer.Text = footers.social.title;
            lbl_tweet.Text = footers.social.tweet;
            lbl_rit_tweeter.Text = footers.social.by;
            pic_box_tweeter.Load("http://ist.rit.edu/assets/img/Social_Icons/twitter.png");
            pic_box_tweeter.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_box_tweeter.Click += (sender1, args) => {
                System.Diagnostics.Process.Start(footers.social.twitter);
            };

            pic_box_facebook.Load("http://ist.rit.edu/assets/img/Social_Icons/facebook.png");
            pic_box_facebook.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_box_facebook.Click += (sender1, args) => {
                System.Diagnostics.Process.Start(footers.social.facebook);
            };

            btn_apply.Text = footers.quickLinks[0].title;
            btn_apply.Click += (sender1, args) => {
                System.Diagnostics.Process.Start(footers.quickLinks[0].href);
            };

            lnk_lbl_abt_site.Text = footers.quickLinks[1].title;
            lnk_lbl_abt_site.Click += (sender1, args) => {
                System.Diagnostics.Process.Start(footers.quickLinks[1].href);
            };


            lnk_lbl_sup.Text = footers.quickLinks[2].title;
            lnk_lbl_sup.Click += (sender1, args) => {
                System.Diagnostics.Process.Start(footers.quickLinks[2].href);
            };

            lnk_lbl_hrs.Text = footers.quickLinks[3].title;
            lnk_lbl_hrs.Click += (sender1, args) => {
                System.Diagnostics.Process.Start(footers.quickLinks[3].href);
            };

            lbl_copyright_title.Text = footers.copyright.title;
            lbl_copyright.AutoSize = true;
            lbl_copyright.MaximumSize = new Size(800, 400);
            String cpyStr = footers.copyright.html;
            //used regular expression to remove html tags
            cpyStr = Regex.Replace(cpyStr, "<p>", "");
            cpyStr = Regex.Replace(cpyStr, "nbsp;|nbsp;", "");
            cpyStr = Regex.Replace(cpyStr, "</p>", "");
            cpyStr = Regex.Replace(cpyStr, "</a>", "");
            cpyStr = Regex.Replace(cpyStr, "<br>", "");
            cpyStr = Regex.Replace(cpyStr, "<a[^>]*>", "");
            lbl_copyright.Text = cpyStr;


            lnk_lbl_news.Text = "News";
            if (news == null)
            {
                string jsonNews = rj.getRestData("/news/");
                news = JToken.Parse(jsonNews).ToObject<News>();
            }
            
            var frm = new Form();
            frm.Text = "News";
            frm.Width = 1000;
            frm.Height = 400;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.BackColor = Color.FromArgb(255, 160, 122);
            frm.AutoScroll = true;
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(950, 0);
            Label lbl1 = new Label();
            lbl1.AutoSize = true;
            lbl1.MaximumSize = new Size(950, 0);

            lnk_lbl_news.Click += (sender1, args) =>
            {
                string str = "";
                lbl1.Text = "News";
                lbl1.ForeColor = Color.Maroon;
                lbl1.Padding = new Padding(400, 20, 0, 0);
                lbl1.Font = new Font("Microsoft Sans Serif", 16);
                frm.Controls.Add(lbl1);
                str += "   ";
                foreach (Older thisOlder in news.older)
                {
                    str += "\n\nDate : " + thisOlder.date;
                    if (thisOlder.title != "" && thisOlder.title != null)
                    {
                        str += "\n\nTitle : " + thisOlder.title;
                    }
                    if (thisOlder.description != "" && thisOlder.description != null)
                     { 
                        str += "\n\nDescription : " + thisOlder.description + "\n\n";
                     }
                }
                lbl.Text += str;
                lbl.Font = new Font("Microsoft Sans Serif", 12);
                lbl.Left = 5;
                lbl.Top = 20;
                frm.Controls.Add(lbl);
                frm.ShowDialog();
            };

        }

    }
}
