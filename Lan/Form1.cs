using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Collections;
using System.Net;



namespace Lan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public string workgroup;

       

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            try
            {


                DirectoryEntry entryPC;
                entryPC = new DirectoryEntry();
                entryPC.Path = "WinNT:" ;

                foreach (DirectoryEntry child in entryPC.Children)
                {


                    if (child.Name != "Schema")
                    {
                        comboBox1.Items.Add(child.Name);
                        //listBox2.Items.Add(child.Name);
                        
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("System.NullReferenceException:"))
                {
                    MessageBox.Show("Select A Workgroup");
                }
                else
                {

                  
                    MessageBox.Show(ex.Message.ToString());
                   
                    
                }
            }

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = listBox2.SelectedItem.ToString();
                //IP
                foreach (IPAddress ip in Dns.GetHostAddresses(listBox2.SelectedItem.ToString()))
                {
                    textBox2.Text = ip.ToString();
                }

 
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            
            }


            

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                   

                textBox1.Text = this.comboBox2.SelectedItem.ToString();
                //IP
                foreach (IPAddress ip in Dns.GetHostAddresses(this.comboBox2.SelectedItem.ToString()))
                {
                    textBox2.Text = ip.ToString();
                }


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {



                PerformInvoke(listBox2, () => { listBox2.Items.Clear(); });
                PerformInvoke(label3, () => { label3.Text = listBox2.Items.Count.ToString(); });
                


                DirectoryEntry entryPC;
                entryPC = new DirectoryEntry();

                entryPC.Path = "WinNT://" + workgroup.ToString();
                backgroundWorker1.ReportProgress(50);
                foreach (DirectoryEntry child in entryPC.Children)
                {


                    if (child.Name != "Schema")
                    {
                       PerformInvoke(listBox2,()=>{listBox2.Items.Add(child.Name);});
                       PerformInvoke(label3, () => { label3.Text = listBox2.Items.Count.ToString(); });
                       PerformInvoke(comboBox2, () => { comboBox2.Items.Add(child.Name); }); 
                        
                        
                        


                    }

                }
                backgroundWorker1.ReportProgress(100);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Object reference not set to an instance of an object."))
                {
                    MessageBox.Show("Select A Workgroup");
                }
                else
                    MessageBox.Show(ex.ToString());

                }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

        }

        public static void PerformInvoke(Control ctrl, Action action)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(action);

            }
            else
            {
                action();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            workgroup = comboBox1.SelectedItem.ToString();
        }

        
        
    }
}
