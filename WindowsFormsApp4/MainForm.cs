﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Load += Form1_Load;
            dgv.EditingControlShowing += onEditingControlShowing;
            dgv.CellBeginEdit += onCellBeginEdit;
            dgv.CellEndEdit += onCellEndEdit;
            dgv.CurrentCellChanged += onCurrentCellChanged;
            dgv.CurrentCellDirtyStateChanged += onCurrentCellDirtyStateChanged;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        private void onCurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"{nameof(onCurrentCellDirtyStateChanged)} {dgv.IsCurrentCellDirty} {dgv.IsCurrentCellInEditMode}");
        }

        private void onCurrentCellChanged(object sender, EventArgs e)
        {
            if(dgv.CurrentCell == null)
            {
                Debug.WriteLine($"{nameof(onCurrentCellChanged)} null");
            }
            else
            {
                Debug.WriteLine($"{nameof(onCurrentCellChanged)} [{dgv.CurrentCell.ColumnIndex}, {dgv.CurrentCell.RowIndex}]");
            }
        }

        private void onCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Debug.WriteLine(nameof(onCellBeginEdit));

#if false
            // ALSO SEEMS TO WORK IF PLACED HERE
            if((dgv.CurrentCell != null) && (_cbEdit != null))
            {
                dgv.CurrentCell.Value = _cbEdit.Text;
            }
#endif
        }

        private void onCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine(nameof(onCellEndEdit));
        }

        private void onEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if(e.Control is DataGridViewComboBoxEditingControl cbEdit)
            {
                CBEdit = cbEdit;
            }
        }
        DataGridViewComboBoxEditingControl _cbEdit = null;
        public DataGridViewComboBoxEditingControl CBEdit
        {
            get => _cbEdit;
            set
            {
                if(!ReferenceEquals(_cbEdit, value))
                {
                    if(_cbEdit != null)
                    {
                        _cbEdit.GotFocus -= onCBEdit_GotFocus;
                        _cbEdit.LostFocus -= onCBEdit_LostFocus;
                    }
                    _cbEdit = value;
                    if (_cbEdit != null)
                    {
                        _cbEdit.GotFocus += onCBEdit_GotFocus;
                        _cbEdit.LostFocus += onCBEdit_LostFocus;
                    }
                }
            }
        }

        private void onCBEdit_GotFocus(object sender, EventArgs e)
        {
            Debug.WriteLine($"CBEdit got focus with text='{_cbEdit.Text}'");
            if (btnWorkaround.Checked)
            {
                if ((dgv.CurrentCell != null) && (_cbEdit != null))
                {
                    dgv.CurrentCell.Value = _cbEdit.Text;
                }
            }
        }

        private void onCBEdit_LostFocus(object sender, EventArgs e)
        {
            Debug.WriteLine($"CBEdit losing focus with text='{_cbEdit.Text}'");
        }
        DataTable dtString1;
        DataTable dtString2;
        DataTable dtString3;
        private void Form1_Load(object sender, EventArgs e)
        {
            // create three combobox columns and put them side-by-side:
            // first column:
            DataGridViewComboBoxColumn dgvcbc1 = new DataGridViewComboBoxColumn();
            dgvcbc1.DataPropertyName = "String1";
            dgvcbc1.Name = "String1";

            dtString1 = new DataTable("String1Options");
            dtString1.Columns.Add("String1Long", typeof(string));

            dtString1.Rows.Add("apple");
            dtString1.Rows.Add("bob");
            dtString1.Rows.Add("clobber");
            dtString1.Rows.Add("dilbert");
            dtString1.Rows.Add("ether");

            dgv.Columns.Insert(0, dgvcbc1);

            dgvcbc1.DisplayMember = dtString1.Columns[0].ColumnName;
            dgvcbc1.ValueMember = dtString1.Columns[0].ColumnName;
            dgvcbc1.DataSource = dtString1;

            dgvcbc1.FlatStyle = FlatStyle.Flat;

            // create the second column:
            DataGridViewComboBoxColumn dgvcbc2 = new DataGridViewComboBoxColumn();
            dgvcbc2.DataPropertyName = "String2";
            dgvcbc2.Name = "String2";

            dtString2 = new DataTable("String2Options");
            dtString2.Columns.Add("String2Long", typeof(string));

            dtString2.Rows.Add("apple");
            dtString2.Rows.Add("bob");
            dtString2.Rows.Add("clobber");
            dtString2.Rows.Add("dilbert");
            dtString2.Rows.Add("ether");

            dgv.Columns.Insert(1, dgvcbc2);

            dgvcbc2.DisplayMember = dtString2.Columns[0].ColumnName;
            dgvcbc2.ValueMember = dtString2.Columns[0].ColumnName;
            dgvcbc2.DataSource = dtString2;

            dgvcbc2.FlatStyle = FlatStyle.Flat;

            // create the third column:
            DataGridViewComboBoxColumn dgvcbc3 = new DataGridViewComboBoxColumn();
            dgvcbc3.DataPropertyName = "String3";
            dgvcbc3.Name = "String3";

            dtString3 = new DataTable("String3Options");
            dtString3.Columns.Add("String3Long", typeof(string));

            dtString3.Rows.Add("apple");
            dtString3.Rows.Add("bob");
            dtString3.Rows.Add("clobber");
            dtString3.Rows.Add("dilbert");
            dtString3.Rows.Add("ether");

            dgv.Columns.Insert(2, dgvcbc3);

            dgvcbc3.DisplayMember = dtString3.Columns[0].ColumnName;
            dgvcbc3.ValueMember = dtString3.Columns[0].ColumnName;
            dgvcbc3.DataSource = dtString3;

            dgvcbc3.FlatStyle = FlatStyle.Flat;
        }

#if false
        // During debugging I reworded this to make sure I
        // understood what was supposed to be happening here.
        string[] values { get; } = new string[]
        {
            "apple",
            "bob",
            "clobber",
            "dilbert",
            "ether"
        };
        private void Form1_Load(object sender, EventArgs e)
        {
            // create three combobox columns and put them side-by-side:

            string colName;
            DataTable options;
            DataGridViewColumn c;

            for (int i = 1; i <= 3; i++)
            {
                colName = $"String{i}";
                options = new DataTable();
                options.Columns.Add(colName, typeof(string));
                foreach (var value in values)
                {
                    options.Rows.Add(value);
                }
                c = new DataGridViewComboBoxColumn()
                {
                    Name = colName,
                    DataPropertyName = colName,
                    DisplayMember = options.Columns[0].ColumnName,
                    ValueMember = options.Columns[0].ColumnName,
                    DataSource = options,
                    FlatStyle = FlatStyle.Flat,
                };
                dgv.Columns.Add(c);
            }
        }
#endif

        SemaphoreSlim _criticalSection = new SemaphoreSlim(1, 1);
        public void SendKeyPlusTab(string keys)
        {
            if (_criticalSection.Wait(0))
            {
                try
                {
                    var nRowsB4 = dgv.Rows.Count;
                    if (!dgv.Focused)
                    {
                        dgv.Focus();
                    }
                    bool first = true;
                    foreach (var key in keys)
                    {
                        SendKeys.SendWait($"{key}\t");
                        if (first)
                        {
                            // Using SendKeys acts slightly different internally from
                            // pressing and releasing physical keys. If the button
                            // clicks are too fast, a new row might not create the way
                            // it should. This line ensures the call value changes.
                            // =======================================================
                            // Force new row - fixes an artifact of automated testing.
                            first = false;
                            dgv.CurrentCell.Value = CBEdit.Text;
                        }
                    }
                    if (dgv.Rows.Count == nRowsB4)
                    {
                        MessageBox.Show(msg);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
                finally
                {
                    _criticalSection.Release();
                }
            }
        }
        const string msg =
@"New bug detected!

The DGV should make a row as soon as a cell 
goes into edit mode. This only seems to
occur when repeating the same button.
        
Alternating the ABC and CDE buttons seems
to work always.";

        private void buttonABC_Click(object sender, EventArgs e)
        {
            SendKeyPlusTab("abc");
        }

        private void buttonCDE_Click(object sender, EventArgs e)
        {
            SendKeyPlusTab("cde");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
        }

        private void btnWorkaround_CheckedChanged(object sender, EventArgs e)
        {
            if(btnWorkaround.Checked)
            {
                btnWorkaround.Text = "Workaround is ON";
            }
            else
            {
                btnWorkaround.Text = "Workaround is OFF";
            }
        }
    }

#if false
    // NORMAL (slow)
    onCurrentCellChanged [0, 0]
    onCellBeginEdit
    CBEdit got focus with text='apple'
    onCurrentCellDirtyStateChanged True True
    onCurrentCellDirtyStateChanged False True
    CBEdit losing focus with text='apple'
    onCellEndEdit
    onCurrentCellChanged [1, 0]
    onCellBeginEdit
    CBEdit got focus with text='apple'
    onCurrentCellDirtyStateChanged True True
    onCurrentCellDirtyStateChanged False True
    CBEdit losing focus with text='bob'
    onCellEndEdit
    onCurrentCellChanged [2, 0]
    onCellBeginEdit
    CBEdit got focus with text='apple'
    onCurrentCellDirtyStateChanged True True
    onCurrentCellDirtyStateChanged False True
    CBEdit losing focus with text='clobber'
    onCellEndEdit
    onCurrentCellChanged [0, 1]

    // PATHOLOGICAL (fast)
    onCellBeginEdit
    CBEdit got focus with text='apple'
    onCurrentCellDirtyStateChanged True True
    onCurrentCellDirtyStateChanged False True
    CBEdit losing focus with text='apple'
    onCellEndEdit
    onCurrentCellChanged [1, 1]
    onCellBeginEdit
    CBEdit got focus with text='bob'
    CBEdit losing focus with text='bob'
    onCellEndEdit
    onCurrentCellChanged [2, 1]
    onCellBeginEdit
    CBEdit got focus with text='clobber'
    CBEdit losing focus with text='clobber'
    onCellEndEdit
    onCurrentCellChanged [0, 2]
#endif
}
