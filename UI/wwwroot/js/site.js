// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Datatabale for role 
$(document).ready(function () {
    dataTable = $("#roleDataTable").DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Administration/GetRole",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "roleId", "visible": false },
            { "data": "roleName", "autoWidth": true },
            {
                "data": "roleId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;Role";
                   /* if (row.roleName == "SuperAdmin") {*/

                    var url = "/Administration/Details/" + row.roleId;
                    var deleteUrl = "/Administration/Delete/" + row.roleId;

                    return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/Administration/Edit/" + row.roleId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

                    //} 
                    
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
            { "data": "listItemCategoryId", "visible": false },
            { "data": "listItemCategoryName", "autoWidth": true },
            {
                "data": "listItemCategoryId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;Category";
                    var url = "/ListItemCategory/Details/" + row.listItemCategoryId;
                    var deleteUrl = "/ListItemCategory/Delete/" + row.listItemCategoryId;
                    return "<a onclick=showPopup('"+ url+ "','" +details+"')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/ListItemCategory/Edit/" + row.listItemCategoryId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";
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
            { "data": "listItemId", "visible": false },
            { "data": "listItemCategory.listItemCategoryName", "autoWidth": true },
            { "data": "listItemName", "autoWidth": true },

            {
                "data": "listItemId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;ListItem";
                    /* if (row.roleName == "SuperAdmin") {*/

                    var url = "/ListItem/Details/" + row.listItemId;
                    var deleteUrl = "/ListItem/Delete/" + row.listItemId;

                    return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/ListItem/Edit/" + row.listItemId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

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
            { "data": "employeeId", "visible": false },
            { "data": "personId", "visible": false},
            { "data": "fullName", "autoWidth": true },
            { "data": "email", "autoWidth": true },
            { "data": "gender", "autoWidth": true },
            { "data": "roleName", "autoWidth": true },
            {
                "data": "employeeId", "orderable": false, "render": function (data, type, row) {
                    var details = "Details&nbsp;View&nbsp;Of&nbsp;Employee";
                    var url = "/Employee/Details/" + row.employeeId;
                    var deleteUrl = "/Employee/Delete/" + row.employeeId;

                    return "<a onclick=showPopup('" + url + "','" + details + "')  class='btn btn-info btn-sm'><i class='fa fa-eye'></i> View</a><a href='/Employee/Edit/" + row.employeeId + "'  class='btn btn-primary btn-sm' style='margin-left:5px' ><i class='far fa-edit'></i> Edit</a><a href='#' class='btn btn-danger btn-sm' style='margin-left:5px' onclick=deleteConfirm('" + deleteUrl + "'); ><i class='far fa-trash-alt'></i> Delete</a>";

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
 