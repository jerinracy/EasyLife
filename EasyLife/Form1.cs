using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyLife
{
    public partial class Form1 : Form
    {
        private string masterUser = "Admin";
        private string masterPassword = "Admin";
        public string login;
        
        public Form1()
        {
            InitializeComponent();
            login= loginText.Text;
            DBDataContext db = new DBDataContext();
            
        }


        
        private void loginbutton_Click(object sender, EventArgs e)
        {
            
            DBDataContext db = new DBDataContext();
            
            admin_info admin = db.admin_infos.SingleOrDefault(x => x.username == loginText.Text && x.password == loginpassword.Text);
            customer_info customer = db.customer_infos.SingleOrDefault(x=> x.username == loginText.Text && x.password == loginpassword.Text);
            if(loginText.Text == "")
            {
                MessageBox.Show("Username is Empty","Warning Message");
            }
            else if(loginpassword.Text == "")
            {
                MessageBox.Show("Passeord is Empty","Warning Message");
            }
            else if (loginText.Text == masterUser && loginpassword.Text == masterPassword)
            {
                this.AdminRegistrationpanel.Visible = true;
            }
            else if(admin != null)
            {
                MainAdminPanel.Visible = true;
            }
            else if(customer != null)
            {
                CustomerPanel.Visible = true;
                //customer_info c_info = db.customer_infos.SingleOrDefault(x => x.username == loginText.Text);
                //order ordr = db.orders.SingleOrDefault(x => x.user_id == c_info.id);
                //if(ordr != null)
                //{
                //    feedback fb = db.feedbacks.SingleOrDefault(x => x.feedback_id == ordr.feedback_id);
                //    if (fb.rating == null)
                //    {
                //        FeedBack Fback = new FeedBack();
                //        Fback.Show();
                //    }
                //}
                
            }
            else
            {
                MessageBox.Show("Username or Password is Wrong", "Warning Message");

            }
        }

        private void signupButton_Click(object sender, EventArgs e)
        {
            CRpanel.Visible = true;
        }

        
        private void CRcancelButton_Click(object sender, EventArgs e)
        {
            CRpanel.Visible = false;
        }

        
        

        

        private void MARcalcelButton_Click_1(object sender, EventArgs e)
        {
            this.AdminRegistrationpanel.Visible = false;
            loginText.Text = "";
            loginpassword.Text = "";
        }

        private void MARRegisterButton_Click(object sender, EventArgs e)
        {
            if (MARusernameText.Text == ""||MARnameText.Text==""||MARageText.Text==""||MARpasswordText.Text==""||MARrepasswordText.Text==""||MARemailText.Text==""||MARphoneText.Text==""||MARlocationText.Text=="")
            {
                MessageBox.Show("One of the field is empty","Warning Message");
            }
            else
            {
                if(MARpasswordText.Text == MARrepasswordText.Text)
                {
                    DBDataContext db = new DBDataContext();
                    admin_info ad_in = new admin_info();
                    ad_in.username = MARusernameText.Text;
                    ad_in.name = MARnameText.Text;
                    ad_in.phone_number = MARphoneText.Text;
                    ad_in.email = MARemailText.Text;
                    ad_in.age = int.Parse(MARageText.Text);
                    ad_in.password = MARpasswordText.Text;

                    location loc = new location();
                    loc.full_address = MARlocationText.Text;

                    ad_in.location = loc;
                    
                    db.admin_infos.InsertOnSubmit(ad_in);
                    db.SubmitChanges();
                    MessageBox.Show("Sucessfully Registered");
                }
                else
                {
                    MessageBox.Show("Password and Re-Type Password Didn't match");
                }
                
            }
        }

        private void CRregisterButton_Click(object sender, EventArgs e)
        {
            if(CRusernameText.Text==""||CRnameText.Text==""||CRpasswordText.Text==""||CRrepasswordText.Text==""||CRageText.Text==""||CRnumberText.Text==""||CRemailText.Text==""||CRlocationText.Text=="")
            {
                MessageBox.Show("One of the field is empty");
            }
            else
            {
                if(CRpasswordText.Text==CRrepasswordText.Text)
                {
                    DBDataContext db = new DBDataContext();
                    customer_info customer = new customer_info();
                    customer.username = CRusernameText.Text;
                    customer.name = CRnameText.Text;
                    customer.age = int.Parse(CRageText.Text);
                    customer.phone_number = CRnumberText.Text;
                    customer.email = CRemailText.Text;
                    customer.password = CRpasswordText.Text;
                    location loc = new location();
                    loc.full_address = CRlocationText.Text;

                    

                    
                    customer.location = loc;
                    //db.locations.InsertOnSubmit(loc);
                    //db.login_infos.InsertOnSubmit(login);
                    db.customer_infos.InsertOnSubmit(customer);
                    db.SubmitChanges();
                    MessageBox.Show("Sucessfully Registered");
                }
            }
        }

        private void MAPlogoutButton_Click(object sender, EventArgs e)
        {
            MainAdminPanel.Visible = false;
            loginText.Text = "";
            loginpassword.Text = "";
        }

        private void MAPcheckButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            var data = from x in db.categories
                       select x.category_name;
            CheckOrderCategoryList.DataSource = data;
            OrderCheckPanel.Visible = true;
        }

        private void CheckingOrderbutton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            

            var order = (from ord in db.orders
                         where ord.status == "Pending"
                         select new {ord.order_id,ord.service_id,ord.user_id}
                         );
            CheckOrderDataGride.DataSource = order;
        }

        private void ReConfirmOrderButton_Click(object sender, EventArgs e)
        {
            reConfirmOrderPanel.Visible = true;
        }

        private void ViewStaffButton_Click(object sender, EventArgs e)
        {
            if(CheckOrderCategoryList.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Category First","Warning Message");
            }
            else
            {
                DBDataContext db = new DBDataContext();
                string item = CheckOrderCategoryList.Text;
                category cat = db.categories.SingleOrDefault(x => x.category_name == item);
                staff_availability available = db.staff_availabilities.SingleOrDefault(x => x.availability_id== 1);
                
                var data = (from staff in db.staff_infos
                            where staff.category_id == cat.category_id && 
                            staff.availability_id == available.availability_id
                            select staff
                            );
                CheckOrderDataGride.DataSource = data;

            }
        }

        private void CheckPanelCancelOrderButton_Click(object sender, EventArgs e)
        {
            if(CheckPanelOrderIDText.Text != "")
            {
                int id = int.Parse(CheckPanelOrderIDText.Text);
                DBDataContext db = new DBDataContext();
                order ordr = db.orders.SingleOrDefault(x => x.order_id == id);
                if(ordr != null)
                {
                    ordr.status = "Cancelled";
                }
                else
                {
                    MessageBox.Show("Order Not Found","Warning Message");
                }
            }
            else
            {
                MessageBox.Show("Enter Order ID Frist","Warning Message");
            }
        }

        private void CheckPaanelConfirmButton_Click(object sender, EventArgs e)
        {
            if(CheckPanelOrderIDText.Text==""||CheckPanelStaffIDText.Text == "")
            {
                MessageBox.Show("Order Id or Staff Id field is Empty","Warning Message");
            }
            else
            {
                DBDataContext db = new DBDataContext();
                int id = int.Parse(CheckPanelOrderIDText.Text);
                int staff = int.Parse(CheckPanelStaffIDText.Text);
                order ordr = db.orders.SingleOrDefault(x=> x.order_id == id);
                ordr.status = "Confirmed";
                staff_info stf = db.staff_infos.SingleOrDefault(x=> x.staff_id == staff);
                stf.availability_id = 2;
                ordr.staff_info = stf;

                //db.orders.
                db.SubmitChanges();
                MessageBox.Show("Order Confirmed","Message");
            }
        }

        private void reConfirmAlterButton_Click(object sender, EventArgs e)
        {
            if(reConfirmID.Text=="" || ReConfirmStaffID.Text == "")
            {
                MessageBox.Show("Order ID or Staff ID is Empty","Warning Message");
            }
            else
            {
                DBDataContext db = new DBDataContext();
                order ordr = db.orders.SingleOrDefault(x => x.order_id == int.Parse(reConfirmID.Text));
                ordr.staff_id = int.Parse(ReConfirmStaffID.Text);
                db.SubmitChanges();
                MessageBox.Show("Order Sucessfully Atlered");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            customer_info c_info = db.customer_infos.SingleOrDefault(x=> x.username == loginText.Text);
            location loc = db.locations.SingleOrDefault(x=> x.location_id == c_info.location_id);
            ProfilePanel.Visible = true;
            ProfileUsernameLeble.Text = c_info.username;
            ProfileNameLabel.Text = c_info.name;
            ProfileAgeLable.Text = (c_info.age).ToString();
            ProfilePhoneNumberLabel.Text = c_info.phone_number;
            ProfileEmailLabel.Text = c_info.email;
            ProfileLocationLabel.Text = loc.full_address;

        }

        private void CPLogoutButton_Click(object sender, EventArgs e)
        {
            CustomerPanel.Visible = false;
            loginText.Text = "";
            loginpassword.Text = "";
        }

        private void ProfileEditButton_Click(object sender, EventArgs e)
        {
            ProfileNameText.Visible = true;
            ProfileAgeText.Visible = true;
            ProfilePhoneText.Visible = true;
            ProfileEmailText.Visible = true;
            ProfileLocationText.Visible = true;
            ProfilepanelSaveButton.Visible = true;
            ProfileEditCancelButton.Visible = true;

            ProfileNameLabel.Visible = false;
            ProfileAgeLable.Visible = false;
            ProfilePhoneNumberLabel.Visible = false;
            ProfileEmailLabel.Visible = false;
            ProfileLocationLabel.Visible = false;

        }

        private void ProfileEditBackButton_Click(object sender, EventArgs e)
        {
            ProfileNameText.Visible = false;
            ProfileAgeText.Visible = false;
            ProfilePhoneText.Visible = false;
            ProfileEmailText.Visible = false;
            ProfileLocationText.Visible = false;
            ProfilepanelSaveButton.Visible = false;
            ProfileEditCancelButton.Visible = false;
        }

        private void ProfilepanelSaveButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            customer_info c_info = db.customer_infos.SingleOrDefault(x => x.username == loginText.Text);
            
            c_info.name = ProfileNameText.Text;
            c_info.age = int.Parse(ProfileAgeText.Text);
            c_info.phone_number = ProfilePhoneText.Text;
            c_info.email = ProfileEmailText.Text;
            location loc = new location();
            loc.full_address = ProfileLocationText.Text;

            c_info.location = loc;
            db.SubmitChanges();
            MessageBox.Show("Profile updated Sucessfully","Message");

            ProfileNameLabel.Visible = true;
            ProfileAgeLable.Visible = true;
            ProfilePhoneNumberLabel.Visible = true;
            ProfileEmailLabel.Visible = true;
            ProfileLocationLabel.Visible = true;

        }

        

        private void OrderServiceBackButton_Click(object sender, EventArgs e)
        {
            OrderServicePanel.Visible = false;
        }

        private void OrderServiceConfirmButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            order ordr = new order();
            ordr.date = OrderServiceDate.Value;
            //service srvs = db.services.SingleOrDefault(x=> x.service_name == orderServiceComboBox.SelectedText);
            ordr.service_id = orderServiceComboBox.SelectedIndex+1;
            customer_info c_info = db.customer_infos.SingleOrDefault(x => x.username == loginText.Text);
            ordr.user_id = c_info.id;
            ordr.status = "Pending";
            db.orders.InsertOnSubmit(ordr);
            db.SubmitChanges();
        }

        private void CPorderServiceButton_Click(object sender, EventArgs e)
        {
            OrderServicePanel.Visible = true;
            DBDataContext db = new DBDataContext();
            var data = from x in db.services
                       select x.service_name;
            orderServiceComboBox.DataSource = data;
        }

        private void OrderHistoryBackButton_Click(object sender, EventArgs e)
        {
            OrderHistoryGrid.Visible = false;
            OrderHistoryBackButton.Visible = false;
        }

        private void CPManageOrderbutton_Click(object sender, EventArgs e)
        {
            ManageOrderpanel.Visible = true;
        }

        private void ManageOrderEditButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            order ordr = db.orders.SingleOrDefault(x=> x.order_id == int.Parse(ManageOrderOrderIDText.Text));
            if(ordr != null)
            {
                ordr.date = ManageOrderDate.Value;
                db.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Order Not Found","Warning message");
            }
        }

        private void ManageOrderViewButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            customer_info c_info = db.customer_infos.SingleOrDefault(x=> x.username == loginText.Text);
            var ordr = (from ord in db.orders
                        where ord.user_id == c_info.id
                        select new {ord.order_id,ord.service,ord.date,ord.status });
            ManageOrderGrid.DataSource = ordr;
        }

        private void ManageOrderCalcelButton_Click(object sender, EventArgs e)
        {
            if(ManageOrderOrderIDText.Text!=null)
            {
                DBDataContext db = new DBDataContext();
                order ordr = db.orders.SingleOrDefault(x => x.order_id == int.Parse(ManageOrderOrderIDText.Text));
                ordr.status = "Cancelled";
                db.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Enter Order ID First","Warning Message");
            }
            
        }

        private void MAPprofileButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            admin_info a_info = db.admin_infos.SingleOrDefault(x=> x.username == loginText.Text);
            location loc = db.locations.SingleOrDefault(x=> x.location_id == a_info.location_id);
            AdminProfilePanel.Visible = true;
            APusername.Text = a_info.username;
            APname.Text = a_info.name;
            APage.Text = a_info.age.ToString();
            APphone.Text = a_info.phone_number;
            APemail.Text = a_info.email;
            APlocation.Text = loc.full_address;
            
        }

        private void MAPstaffListButton_Click(object sender, EventArgs e)
        {
            viewStaffPanel.Visible = true;

        }

        private void MAPaddStaffButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            var data = from x in db.categories
                       select x.category_name;
            SRcategorylist.DataSource = data;

            StaffAddingPanel.Visible = true;
        }

        private void MAPaddAdminButton_Click(object sender, EventArgs e)
        {
            
            AdminRegistrationpanel.Visible = true;
        }

        private void MAPaddCategoryButton_Click(object sender, EventArgs e)
        {
            MAPaddCategoryButton.Visible = false;
            CategoryAddButton.Visible = true;
            categoryText.Visible = true;
            categoryDone.Visible = true;
            
        }

        private void CategoryAddButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            category cat = new category();
            cat.category_name = categoryText.Text;
            db.categories.InsertOnSubmit(cat);
            db.SubmitChanges();
        }

        private void categoryDone_Click(object sender, EventArgs e)
        {
            CategoryAddButton.Visible = false;
            categoryText.Visible = false;
            categoryDone.Visible = false;
            MAPaddCategoryButton.Visible = true;
        }

        private void viewstaffSearch_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            category cat = db.categories.SingleOrDefault(x=> x.category_name == viewstaffcategory.SelectedText);
            var data = (from x in db.staff_infos
                        where x.name.StartsWith(viewstaffSearchText.Text) &&
                        x.category_id == cat.category_id
                        select x);
            viewstaffGrid.DataSource = data;
        }

        private void viewStaffBackButton_Click(object sender, EventArgs e)
        {
            viewStaffPanel.Visible = false;
        }

        private void stafflistviewstaffButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            var data = from x in db.staff_infos
                       select x;
            viewstaffGrid.DataSource = data;
        }

        private void SRregistrationButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            staff_info stf = new staff_info();
            stf.name = SRnameText.Text;
            stf.age = int.Parse(SRageText.Text);
            stf.phone_number = SRphoneNumberText.Text;
            location loc = new location();
            loc.full_address = SRlocationText.Text;
            category cat = db.categories.SingleOrDefault(x=> x.category_name == SRcategorylist.SelectedText);
            stf.location = loc;
            stf.category_id = cat.category_id;
            stf.availability_id = 1;
            db.staff_infos.InsertOnSubmit(stf);
            db.SubmitChanges();
        }

        private void SRbackButton_Click(object sender, EventArgs e)
        {
            StaffAddingPanel.Visible = false;
        }

        private void CPHistoryButton_Click(object sender, EventArgs e)
        {
            OrderHistoryGrid.Visible = true;
            OrderHistoryBackButton.Visible = true;
            DBDataContext db = new DBDataContext();
            customer_info c_info = db.customer_infos.SingleOrDefault(x=> x.username == loginText.Text);
            var data = from x in db.orders
                       where x.user_id == c_info.id
                       select new {x.order_id,x.service_id,x.status };
            OrderHistoryGrid.DataSource = data;
        }

        private void viewStaffRemoveButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            staff_info staf = db.staff_infos.SingleOrDefault(x => x.staff_id == int.Parse(viewstaffSearchText.Text));
            if(staf != null)
            {
                db.staff_infos.DeleteOnSubmit(staf);
                db.SubmitChanges();
            }
        }

        private void CheckpanelBackButton_Click(object sender, EventArgs e)
        {
            OrderCheckPanel.Visible = false;
        }

        private void viewConfirmedOrderBack_Click(object sender, EventArgs e)
        {
            reConfirmOrderPanel.Visible = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminProfilePanel.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            APnameText.Visible = true;
            APageText.Visible = true;
            APphoneText.Visible = true;
            APemailText.Visible = true;
            APlocationText.Visible = true;
            AdminProfileSaveButton.Visible = true;
            AdminProfileEditCancelButton.Visible = true;
            APname.Visible = false;
            APage.Visible = false;
            APphone.Visible = false;
            APemail.Visible = false;
            APlocation.Visible = false;
        }

        private void AdminProfileEditCancelButton_Click(object sender, EventArgs e)
        {
            APnameText.Visible = false;
            APageText.Visible = false;
            APphoneText.Visible = false;
            APemailText.Visible = false;
            APlocationText.Visible = false;
            AdminProfileSaveButton.Visible = false;
            AdminProfileEditCancelButton.Visible = false;
            APname.Visible = true;
            APage.Visible = true;
            APphone.Visible = true;
            APemail.Visible = true;
            APlocation.Visible = true;

        }

        private void AdminProfileSaveButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            admin_info a_info = db.admin_infos.SingleOrDefault(x=> x.username == loginText.Text);
            location loc = db.locations.SingleOrDefault(x=> x.location_id == a_info.location_id);
            a_info.name = APnameText.Text;
            a_info.age = int.Parse(APageText.Text);
            a_info.phone_number = APphoneText.Text;
            a_info.email = APemailText.Text;
            loc.full_address = APlocationText.Text;
            db.SubmitChanges();

            APnameText.Visible = false;
            APageText.Visible = false;
            APphoneText.Visible = false;
            APemailText.Visible = false;
            APlocationText.Visible = false;
            AdminProfileSaveButton.Visible = false;
            AdminProfileEditCancelButton.Visible = false;

            APname.Visible = true;
            APage.Visible = true;
            APphone.Visible = true;
            APemail.Visible = true;
            APlocation.Visible = true;

            MessageBox.Show("Profile Updated","Sucess Message");
            APname.Text = a_info.name;
            APage.Text = a_info.age.ToString();
            APphone.Text = a_info.phone_number;
            APemail.Text = a_info.email;
            APlocation.Text = loc.full_address;
        }

        private void viewConfirmedOrderButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            var data = from x in db.orders
                       where x.status == "Confirmed"
                       select new {x.order_id, x.service_id,x.status };
            reConfirmGride.DataSource = data;
        }

        private void reCancelButton_Click(object sender, EventArgs e)
        {
            if(reConfirmID.Text == "")
            {
                MessageBox.Show("Enter Order ID that You want to Cancel");
            }
            else
            {
                DBDataContext db = new DBDataContext();
                order ordr = db.orders.SingleOrDefault(x=> x.order_id == int.Parse(reConfirmID.Text));
                ordr.status = "cancelled";
                db.SubmitChanges();
            }
        }

        private void ReconfirmStaff_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            var data = from x in db.staff_infos
                       where x.availability_id == 1
                       select new {x.staff_id, x.name,x.phone_number,x.location };
            reConfirmGride.DataSource = data;
        }

        private void MAPaddService_Click(object sender, EventArgs e)
        {
            MAPaddService.Visible = false;
            ServiceAddButton.Visible = true;
            ServiceAddText.Visible = true;
            ServiceAddDoneButton.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            service srvc = new service();
            srvc.service_name = ServiceAddText.Text;
            db.services.InsertOnSubmit(srvc);
            db.SubmitChanges();
        }

        private void ServiceAddDoneButton_Click(object sender, EventArgs e)
        {
            MAPaddService.Visible = true;
            ServiceAddButton.Visible = false;
            ServiceAddText.Visible = false;
            ServiceAddDoneButton.Visible = false;
        }

        private void ProfileEditCancelButton_Click(object sender, EventArgs e)
        {
            ProfileNameText.Visible = false;
            ProfileAgeText.Visible = false;
            ProfilePhoneText.Visible = false;
            ProfileEmailText.Visible = false;
            ProfileLocationText.Visible = false;
            ProfilepanelSaveButton.Visible = false;
            ProfileEditCancelButton.Visible = false;

            ProfileNameLabel.Visible = true;
            ProfileAgeLable.Visible = true;
            ProfilePhoneNumberLabel.Visible = true;
            ProfileEmailLabel.Visible = true;
            ProfileLocationLabel.Visible = true;
        }

        private void ProfileBackButton_Click(object sender, EventArgs e)
        {
            ProfilePanel.Visible = false;
        }
        
    }
}
