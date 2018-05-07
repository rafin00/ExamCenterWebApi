$(function(){
	
	$('#createbtn').click(function(){
	 var user = {UserName : $('#UserName').val(),Password:$('#Password').val(),FullName:$('#FullName').val(),UserType:'Student'};
    console.log(user)
	$.ajax({
        type: "POST",
        data :JSON.stringify(user),
			url: 'http://localhost:60270/api/login/CreateUser',
			 async: false,
			 contentType: "application/json",
			headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					$('#msg').html("Created");
					
					
					
				}
				else if(xmlhttp.status==409)
				{
					$('#msg').html("UserName Already Exists");
				}else 
				{$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	});
	
});