// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Datatabale for role 
$(document).ready(function () {
    dataTable = $("#roleDataTable").DataTable({
        "responsive": true,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Administration/GetRole",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "roleId", "name":"RoleId", "visible": false },
            { "data": "encryptedId", "visible": false },
            { "data": "roleName", "name": "RoleName", "autoWidth": true },
            {
                "data": "roleId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;Role";
                    var url = "/Administration/Details/" + row.encryptedId;
                    var deleteUrl = "/Administration/Delete/" + row.encryptedId;
                    if (row.roleName != "SuperAdmin") {
                        return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/Administration/Edit/" + row.encryptedId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

                    } else {
                        return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a>";
                    }
                    
                }
            },

        ],
        "language": {

            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
});

//Datatabale for List Item Category 
$(document).ready(function () {
    dataTable = $("#categoryDataTable").DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ListItemCategory/GetAllCategory",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "encryptedId", "visible": false },
            { "data": "listItemCategoryName", "autoWidth": true },
            {
                "data": "encryptedId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;Category";
                    var url = "/ListItemCategory/Details/" + row.encryptedId;
                    var deleteUrl = "/ListItemCategory/Delete/" + row.encryptedId;
                    return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/ListItemCategory/Edit/" + row.encryptedId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";
                }
            },

        ],
        "language": {

            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
});

//Datatabale for ListItem 
$(document).ready(function () {
    dataTable = $("#listItemDataTable").DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ListItem/GetAllListItems",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "encryptedId", "visible": false },
            { "data": "listItemCategory.listItemCategoryName", "autoWidth": true },
            { "data": "listItemName", "autoWidth": true },

            {
                "data": "encryptedId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;ListItem";
                    /* if (row.roleName == "SuperAdmin") {*/

                    var url = "/ListItem/Details/" + row.encryptedId;
                    var deleteUrl = "/ListItem/Delete/" + row.encryptedId;

                    return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/ListItem/Edit/" + row.encryptedId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

                    //} 

                }
            },

        ],
        "language": {

            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
});

$(document).ready(function () {
    dataTable = $("#employeeDataTable").DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Employee/GetAllEmployees",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "encryptedId", "visible": false },
            { "data": "personId", "visible": false},
            { "data": "fullName", "autoWidth": true },
            { "data": "email", "autoWidth": true },
            { "data": "gender", "autoWidth": true },
            { "data": "roleName", "autoWidth": true },
            {
                "data": "encryptedId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;Employee";
                    var url = "/Employee/Details/" + row.encryptedId;
                    var deleteUrl = "/Employee/Delete/" + row.encryptedId;
                    if (row.roleName != "SuperAdmin") {
                        return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/Employee/Edit/" + row.encryptedId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

                    } else {
                        return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a>";
                    }
                    //return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/Employee/Edit/" + row.employeeId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

                }
            },

        ],
        "language": {

            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
});

//for detail view popup
showPopup = (url, details) => {
    $.ajax({
        type: 'GET',
        url: url,
       
        success: function (res) {
         
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(details);
            $('#form-modal .modal-header').css({ "background-color": "#007bff", "color": "#ffff" });
            $('#form-modal').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}


var deleteConfirm = function (val) {
    $('#idToDelete').text(val);
    console.log(val);
    $('#delete-conformation').modal('show');
}
   
  
function deleteData() {
    var url = $('#idToDelete').text();
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            if (data.success === true) {
                $("#delete-conformation").modal('hide');
                $('#categoryDataTable').DataTable().ajax.reload();
                $('#roleDataTable').DataTable().ajax.reload();
                $('#listItemDataTable').DataTable().ajax.reload();
                $('#employeeDataTable').DataTable().ajax.reload();
                toastr.success(data.message, 'Success Alert', { timeout: 300 });
            } else {
                $("#delete-conformation").modal('hide');
                toastr.error(data.message, 'Warning Alert', { timeout: 300 });
            }
        },
        error: function (err) {
            console.log(err);
            toastr.error(err, 'Error Alert', { timeout: 300 });
        }
    });
}
 