$(function(){
	
	if(localStorage.getItem('usertype')=='Teacher'){
		window.location.href = "Teacher/TeacherHome.html";
	}
	else if(localStorage.getItem('usertype')=='Student'){
		window.location.href = "Student/StudentHome.html";
	}
	
	$('#logbtn').click(function(){
		
		
		
		 var user = {UserName : $('#UserName').val(),Password:$('#PassWord').val()};
    $.ajax({
        type: "POST",
        data :JSON.stringify({UserName : $('#UserName').val(),Password:$('#PassWord').val()}),
        url: 'http://localhost:60270/api/login',
		 async: false,
		headers: {
    "Authorization": "Basic " + btoa($('#UserName').val() + ":" + $('#PassWord').val())
  },
        contentType: "application/json",
		complete: function(xmlhttp){
				if(xmlhttp.status == 200)
				{localStorage.setItem("username",$('#UserName').val());
					localStorage.setItem("password",$('#PassWord').val());
					localStorage.setItem("usertype",xmlhttp.responseJSON.UserType);
					
					
					if(xmlhttp.responseJSON.UserType=='Teacher')
					window.location.href = "Teacher/TeacherHome.html";
				else if(xmlhttp.responseJSON.UserType=='Student')
					window.location.href = "Student/StudentHome.html";
				else if(xmlhttp.responseJSON.UserType=='Admin')
					window.location.href = "Admin/AdminPanel.html";
				
					
					
				}
				else
				{
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
		}
    });
		
	});
	
	$('#signup').click(function(){
	window.location.href = "SignUp.html";
	});
	//login end
	
});