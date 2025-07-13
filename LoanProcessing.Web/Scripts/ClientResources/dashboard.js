
const token = localStorage.getItem('token');
const userId = localStorage.getItem('userId');
const username = localStorage.getItem('username') || 'User';  


var role = (function () {
    if (!token) return 'User';
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload['role'] || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 'User';
    } catch (e) {
        return 'User';
    }
})();


Ext.onReady(() => {
    fetchLoanApplications(apps => {
        if (apps.length > 0) {
            App.loanStore.loadData(apps);
            App.loanGrid.show();
        } else {
            App.loanStore.removeAll();
            App.loanGrid.hide();
            if (role !== "Admin") {
                Ext.Msg.alert('Info', 'არ მოიძებნა განაცხადი');
            }
        }
    });
});

function fetchLoanApplications(callback) {
    const url = role === "Approver"
        ? 'https://localhost:7115/api/LoanApplication/GetAllLoanApplicationsList'
        : 'https://localhost:7115/api/LoanApplication/GetLoanApplicationsList?UserId=' + userId;

    Ext.Ajax.request({
        url,
        method: 'GET',
        headers: { Authorization: 'Bearer ' + token },
        success: function (response) {
            const data = Ext.decode(response.responseText);
            const apps = Array.isArray(data) ? data : (data?.apps ?? []);
            callback(apps);
        },
        failure: function () {
            Ext.Msg.alert('Error', 'Cannot load loan applications.');
        }
    });
}

function showLoanForm() {
    App.loanForm.getForm().reset();
    App.loanFormWindow.setTitle("სესხის განაცხადის ფორმა");
    App.loanFormWindow.show();
}

function saveLoan() {
    const form = App.loanForm.getForm();
    if (!form.isValid()) return;

    let values = form.getValues();
    values.loanType = parseInt(values.loanType, 10);
    values.amount = parseFloat(values.amount);
    values.period = parseInt(values.period, 10);
    values.userId = parseInt(localStorage.getItem('userId'), 10);

    delete values._loanType_state;
    delete values._currency_state;

    const isEditing = !!values.id && parseInt(values.id) > 0;

    const url = isEditing
        ? 'https://localhost:7115/api/LoanApplication/UpdateLoanApplication'
        : 'https://localhost:7115/api/LoanApplication/CreateLoanApplication';

    const method = isEditing ? 'PUT' : 'POST';

    Ext.Ajax.request({
        url: url,
        method: method,
        jsonData: values,
        headers: {
            Authorization: 'Bearer ' + token
        },
        success: function () {
            App.loanFormWindow.hide();
            reloadLoanApplications(values.userId);
            Ext.Msg.alert('Success', isEditing ? 'განაცხადი განახლდა' : 'განაცხადი შეინახა');
        },
        failure: function () {
            Ext.Msg.alert('Error', 'შეცდომა ' + (isEditing ? 'განახლებისას' : 'შენახვისას'));
        }
    });
}

function reloadLoanApplications(userId) {
    var url = role === "Approver"
        ? 'https://localhost:7115/api/LoanApplication/GetAllLoanApplicationsList'
        : 'https://localhost:7115/api/LoanApplication/GetLoanApplicationsList?UserId=' + userId;
    Ext.Ajax.request({
        url: url,
        method: 'GET',
        headers: {
            Authorization: 'Bearer ' + token
        },
        success: function (response) {
            var data = Ext.decode(response.responseText);
            var apps = Array.isArray(data) ? data : (data?.apps ?? []);

            if (apps.length > 0) {
                App.loanStore.loadData(apps);
                App.loanGrid.show();
            } else {
                App.loanStore.removeAll();
                App.loanGrid.hide();
                if (role !== "Admin") {
                    Ext.Msg.alert('Info', 'არ მოიძებნა განაცხადი');
                }
            }
        },
        failure: function () {
            Ext.Msg.alert('Error', 'Failed to load loan applications.');
        }
    });
}


function editLoan(record) {
    if (record.data.status !== 0) {
        Ext.Msg.alert('Info', 'მხოლოდ "Processing" სტატუსის განაცხადის რედაქტირებაა შესაძლებელი.');
        return;
    }

    App.loanForm.getForm().setValues({
        id: record.data.id,
        loanType: record.data.loanType,
        amount: record.data.amount,
        currency: record.data.currency,
        period: record.data.period
    });

    App.loanFormWindow.setTitle("განაცხადის რედაქტირება");
    App.loanFormWindow.show();
}

function approveLoan(record) {
    Ext.Ajax.request({
        url: 'https://localhost:7115/api/LoanApplication/ApproveLoanApplication',
        method: 'PUT',
        jsonData: { Id: record.data.id },
        headers: {
            Authorization: 'Bearer ' + token
        },
        success: function () {
            Ext.Msg.alert('დადასტურება', 'განაცხადი დადასტურდა');
            reloadLoanApplications(userId);
        },
        failure: function () {
            Ext.Msg.alert('Error', 'შეცდომა დადასტურებისას');
        }
    });
}

function rejectLoan(record) {
    Ext.Msg.confirm('გსურს დაუარება?', 'დარწმუნებული ხარ?', function (btn) {
        if (btn === 'yes') {
            Ext.Ajax.request({
                url: 'https://localhost:7115/api/LoanApplication/RejectLoanApplication',
                method: 'DELETE',
                jsonData: { Id: record.data.id },
                headers: {
                    Authorization: 'Bearer ' + token
                },
                success: function () {
                    Ext.Msg.alert('უარყოფა', 'განაცხადი უარყოფილია');
                    reloadLoanApplications(userId);
                },
                failure: function () {
                    Ext.Msg.alert('Error', 'შეცდომა უარყოფისას');
                }
            });
        }
    });
}

function submitLoan(record) {
    const id = record.get("id");

    Ext.Ajax.request({
        url: 'https://localhost:7115/api/LoanApplication/SubmitLoanApplication',
        method: 'POST',
        jsonData: { Id: id },
        headers: {
            Authorization: 'Bearer ' + token
        },
        success: function () {
            Ext.Msg.alert('Success', 'განაცხადი გადაგზავნილია');
            location.reload();
        },
        failure: function () {
            Ext.Msg.alert('Error', 'შეცდომა გადაგზავნისას');
        }
    });
}

function logout() {

    localStorage.removeItem('token');
    localStorage.removeItem('userId');

    document.cookie = "userRole=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    document.cookie = "userId=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

    window.location.href = '/Account/Login';
}

