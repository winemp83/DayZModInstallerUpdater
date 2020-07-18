using DayZToolIniFile.Model;
using MyICommand;
using System;
using System.Collections.Generic;
using System.Text;
using XmlIni;

namespace DayZToolIniFile.ViewModel
{
    public class AddEditIniViewModel : Config
    {
        public AddEditIniViewModel()
        {
            CancelCommand = new MyICommand.MyICommand(OnCancel);
            SaveCommand = new MyICommand.MyICommand(OnSave, CanSave);
        }

        private bool _EditMode;

        public bool EditMode
        {
            get { return _EditMode; }
            set { SetProperty(ref _EditMode, value); }
        }

        private AddEditIniModel _Customer;

        public AddEditIniModel Customer
        {
            get { return _Customer; }
            set { SetProperty(ref _Customer, value); }
        }

        private AddEditIniModel _editingCustomer = null;

        public void SetCustomer(AddEditIniModel cust)
        {
            _editingCustomer = cust;

            if (Customer != null) Customer.ErrorsChanged -= RaiseCanExecuteChanged;
            Customer = new AddEditIniModel();
            Customer.ErrorsChanged += RaiseCanExecuteChanged;
            CopyCustomer(cust, Customer);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChange();
        }

        public MyICommand.MyICommand CancelCommand { get; private set; }
        public MyICommand.MyICommand SaveCommand { get; private set; }

        public event Action Done = delegate { };

        private void OnCancel()
        {
            Done();
        }

        private void OnSave()
        {
            Add(Customer.Key, Customer.Value);
        }

        private bool CanSave()
        {
            return !Customer.HasErrors;
        }
    }
}