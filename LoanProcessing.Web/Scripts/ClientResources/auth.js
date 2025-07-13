function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
}

function setTokenAndRedirect(data) {
    localStorage.setItem('userId', data.userId);
    localStorage.setItem('token', data.token);

    var claims = parseJwt(data.token);
    var role = claims['role'] || claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 'User';
    document.cookie = 'userRole=' + role + '; path=/; max-age=' + (24 * 60 * 60);

    var url = (role === 'Approver')
        ? 'https://localhost:7115/api/LoanApplication/GetAllLoanApplicationsList'
        : 'https://localhost:7115/api/LoanApplication/GetLoanApplicationsList?UserId=' + data.userId;

    if (role === 'Admin') {
        window.location.href = '/Dashboard/Index';
        return;
    }
    Ext.Ajax.request({
        url: url,
        method: 'GET',
        disableCaching: true,
        headers: {
            'Authorization': 'Bearer ' + data.token
        },
        success: function (resp) {
            var appsData = Ext.decode(resp.responseText);
            var apps = Array.isArray(appsData) ? appsData : (appsData?.apps ?? []);

            if (apps.length > 0) {
                window.location.href = '/Dashboard/Index';
            } else {
                window.location.href = '/Dashboard/Empty';
            }
        },
        failure: function () {
            Ext.Msg.alert('Error', 'Failed to check loan applications.');
        }
    });
}