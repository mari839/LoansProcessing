Ext.onReady(function () {
    Ext.create('Ext.form.Panel', {
        title: 'Login / Register',
        width: 400,
        bodyPadding: 10,
        renderTo: Ext.getBody(),   // put the form inside the page body
        items: [
            {
                xtype: 'textfield',
                name: 'username',
                fieldLabel: 'Username',
                allowBlank: false
            },
            {
                xtype: 'textfield',
                name: 'password',
                inputType: 'password',
                fieldLabel: 'Password',
                allowBlank: false
            }
            // Add more fields here if you want for registration (e.g., email)
        ],
        buttons: [
            {
                text: 'Login',
                handler: function () {
                    var form = this.up('form').getForm(); // get the form instance
                    if (form.isValid()) {  // check if fields are valid (not empty)
                        var values = form.getValues(); // get input data
                        Ext.Ajax.request({
                            url: 'https://localhost:7115/api/authentication/login', // your API endpoint
                            method: 'POST',
                            jsonData: values, // send data as JSON
                            success: function (response) {
                                var data = Ext.decode(response.responseText);
                                Ext.Msg.alert('Success', 'Welcome, ' + data.fullName);
                                // You can redirect to dashboard or loans page here
                            },
                            failure: function () {
                                Ext.Msg.alert('Error', 'Login failed. Please check your credentials.');
                            }
                        });
                    }
                }
            },
            {
                text: 'Register',
                handler: function () {
                    var form = this.up('form').getForm();
                    if (form.isValid()) {
                        var values = form.getValues();
                        Ext.Ajax.request({
                            url: 'https://localhost:7115/api/authentication/RegisterUser',
                            method: 'POST',
                            jsonData: values,
                            success: function () {
                                Ext.Msg.alert('Success', 'Registration completed successfully.');
                            },
                            failure: function () {
                                Ext.Msg.alert('Error', 'Registration failed. Try again.');
                            }
                        });
                    }
                }
            }
        ]
    });
});
