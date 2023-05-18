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
    public partial class FeedBack : Form
    {
        public FeedBack()
        {
            InitializeComponent();
            
        }

        private void FeedBackSubmitButton_Click(object sender, EventArgs e)
        {
            DBDataContext db = new DBDataContext();
            Form1 f1 = new Form1();
            customer_info c_info = db.customer_infos.SingleOrDefault(x=> x.username == f1.login);
            order ordr = db.orders.SingleOrDefault(x=> x.user_id == c_info.id);
            feedback fb = db.feedbacks.SingleOrDefault(x=> x.feedback_id == ordr.feedback_id);
            fb.rating = int.Parse(radioButton1.Text);
            fb.comment = FeedbackComment.Text;
            db.feedbacks.InsertOnSubmit(fb);
            db.SubmitChanges();
        }
    }
}
