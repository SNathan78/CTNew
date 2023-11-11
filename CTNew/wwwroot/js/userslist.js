$(document).ready(function () {
    
    GetUsers();
    
});

function GetUsers() {

       $.ajax({
        url: '/Users/GetUsers',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
           success: function (response) {
            
            if (response == undefined || response.length == 0 ) {
                var object = '';
                object += '<tr>';
                object += '<td colspan="7">' + 'Users not available' + '</td></tr>';
                $('#tblBody').html(object);
    
            }
            else {
                var object = '';
                $.each(response, function (index, item) {
                    
                    
                   // console.log("data: " + JSON.stringify(item));
                    
                    object += '<tr>';
                    object += '<td>'+ item.firstName + '</td>';
                    object += '<td>' + item.lastName + '</td>';
                    
                    var gender = (item.genderId == 1) ? 'Female' : 'Male';
                    object += '<td>' + gender + '</td>';
                    object += '<td>' + item.cityName + '</td>';
                    object += '<td>' + item.stateName + '</td> ';
                    object += '<td> <a href="#" class="btn btn-primary" data-toggle="ajax-modal" data-target="#EditUser" onClick="Edit(' + item.userId + ')" >Edit </a></td>';
                    //object += '<td>' + '<button type="button" class="btn btn-primary" data-toggle="ajax-modal" data-target="#EditUser" data-url="@Url.Action($"Edit/{' + item.UserId +' } ")" ></td>';
                    object += '<td> <a href="#" class="btn btn-primary" id="ConfirmDelete" onClick="Delete(' + item.userId + ')" >Delete </a></td></tr>';
                });
                
                $('#tblBody').html(object);
            }
            
        },
        error: function () {

            alert('Error Unable to read the data');
        }
    });
    
}

//Edit
function Edit(id) {
    
    $.ajax({
        url: '/Users/EditUser?id='+ id,
        type: 'get',
        datatype: 'json',
        contentType: 'application/json',
        success: function (response) {
            //console.log(JSON.stringify(response));
            if (response == undefined || response.length == 0) {
                var object = '';
                object += '<tr>';
                object += '<td colspan="7">' + 'Users not available' + '</td></tr>';
                $('#tblBody').html(object);

            }
            else {

                $('#EditUser').modal('show');
                $('#UserId').val(response.userId);
                $('#FirstName').val(response.firstName);
                $('#LastName').val(response.lastName);
                //$('#Gender').val(response.gender);
                if (response.genderId == "1") {
                    $("input[name=gender][value=" + response.genderId + "]").attr('checked', 'checked');
                }
                else {

                    $("input[name=gender][value=" + response.genderId + "]").attr('checked', 'checked');
                }

//                $('#StateId').val(response.stateId);
 //               $('#CityId').val(response.cityId);


                
                $('#StateId').val(response.stateId);
                console.log(response.stateId);
                loadCities(response.stateId, response.cityId);
                $('#CityId').val(response.cityId)
               //$('#StateId').val(response.stateId).change();
                


              //  console.log($('#StateId').text());

               
                


            }

        },
        error: function () {

            alert('Error Unable to read the data');
        }
    });

        

        }
function loadCities(stateid, citiId) {
    //alert('load cities triggered');
    //alert(id);
    $.ajax({
        url: '/Users/GetCities?id=' + stateid,
        type: 'get',
        datatype: 'json',
        contentType: 'application/json',
        success: function (response) {
            $('#CityId').empty();
            $('#CityId').append("<option> Please Select </option>");

            var list = response;
            console.log(JSON.stringify(response));
            $.each(list, function (index, row) {
                if (index == 0) {
                    $('#CityId').append("<option value='" + row.value + "' selected='selected'>" + row.text + "</option>");
                } else {
                    $('#CityId').append("<option value='" + row.value + "'>" + row.text + "</option>")
                }
                $('#CityId').val(citiId);
            });
        },

        error: function () {

            //alert('Error Unable to read the data in loadcities');
        }
    })


}

//Delete
function Delete(id) {

    if (confirm('Are you sure?')) {
        $.ajax({
            url: '/Users/DeleteUser?id=' + id,
            type: 'post',
            contentType:'application/json',
            success: function (response) {
                if (response == null || response == undefined) {
                    alert('Unable to delete the data');
                }
                else {
                    GetUsers();
                    alert('Record deleted Successfully..!!');

                }

            
            },

            error: function () {

                alert('Error Unable to read the data');
            }
        });


    }
}

function AddMod(){ 
//$('#btnAdd').click(function () {
    //alert('add triggered');
    $('#EditUser').modal('show');
    $('#ModalTitle').text('Add User');
    $("#Save").show();
    $("#Update").hide();
    $.validator.unobtrusive.parse("#EditUser");
//});
}
function Close() {
    $('#EditUser').modal('hide');
    location.reload(true);
}



