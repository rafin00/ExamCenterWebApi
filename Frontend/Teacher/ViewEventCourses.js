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
		
		//evnt name
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId'),
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					//var output="<table border=1 cellspacing=0 cellpadding=0><tr><th>Event Name</th><th>Start Time</th><th>End Time</th><th></th><th></th></tr>";
					//for(var i=0;i<result.length;i++)
						
						
					var	EvntName =result.EvntName;
						
					//evnt courses
					$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<div>'+EvntName+'</div><div>Courses</div><a href="AddEventCourse.html?EvntId='+getUrlParameter('EvntId')+'">Add New Course</a>';
					for(var i=0;i<result.length;i++)
					{
						output+='<div><pre>'+result[i].CourseName+' <Button class="viewquestion" value="'+result[i].CourseName+'">View Questions</Button> <Button class="deletecourse" value="'+result[i].CourseName+'">Delete Course</Button></pre></div>'
					}
						
					
						$('#body').html(output);
					
				
				
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html('<a href="AddEventCourse.html?EvntId='+getUrlParameter('EvntId')+'">Add New Course</a></br>No Courses.');
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
					
					//evnt courses end
				
				
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>No Such Events.");
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	}
	load();
	$('.logout').click(function(){
	localStorage.setItem("username",'');
					localStorage.setItem("password",'');
					window.location.href = "../index.html";
	});
	

	//view quesions button
	$( ".viewquestion" ).click(function(){
		console.log('qqq');
		var coursename = $(this).val();
		window.location.href = 'ViewEventCourseQuestion.html?EvntId='+getUrlParameter('EvntId')+'&&CourseName='+coursename;

	});
	//view quesions button end
	
	//delete course btn
	$( ".deletecourse" ).click(function(){
		var answer = confirm('Are you sure you want to delete this course?');
		if (answer)
		{
			//delete ques
			$.ajax({
				 type: "Delete",
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId')+'/courses/'+$(this).val(),
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==204)
				{
					location.reload();
					
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
			//delete ques end
			console.log($(this).val());
		}
		else
		{
			console.log('cancel');
		}
	});
	//delete course btn end
	
	
});