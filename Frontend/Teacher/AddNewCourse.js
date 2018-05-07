$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Teacher')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username');
	var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
	};
	
	
	var load = function(){
		var EvntName="";
		var output='<table border=0><tr ><th colspan=2>New Course</th></tr><tr><td>Course Name:</td><td><Input required id="CourseName" ></td></tr><tr align=center ><td colspan=2><Button id="submit">Submit</Button></td></tr></table>'
				$('#body').html(output);
		
		
	
	}
	load();
	$('.logout').click(function(){
	localStorage.setItem("username",'');
					localStorage.setItem("password",'');
					window.location.href = "../index.html";
	});
	
$('#submit').click(function(){
	
	 var course = {CourseName : $('#CourseName').val(),TeacherUserName:username};
		console.log(course);
		
		$.ajax({
        type: "POST",
        data :JSON.stringify(course),
			url: 'http://localhost:60270/api/teachers/'+username+'/courses',
			 async: false,
			 contentType: "application/json",
			headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==201)
				{
					$('#msg').html("Created Sucessfully");
					
					
					
				}else if(xmlhttp.status==409)
				{
					$('#msg').html("CourseName Already Exists");
					
					
					
				}
				else 
				{console.log(xmlhttp);
					$('#msg').html(xmlhttp.status + ": " + xmlhttp.responseJSON.Message);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	});
	
	
	
	
	
});