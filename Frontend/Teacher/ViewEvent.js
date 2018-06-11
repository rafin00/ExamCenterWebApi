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
						var sttime = result.EvntSdt.split('T');
						var endtime = result.EvntEdt.split('T');
						
					var	output ='<div>'+result.EvntName+'</div><div>Start Time : '+sttime[0]+' '+sttime[1]+'</div><div>End Time : '+endtime[0]+' '+endtime[1]+'</div><div><a href="ViewEventCourses.html?EvntId='+result.EvntId+'">View Event Courses</a></div><div><a href="AssignStudents.html?EvntId='+getUrlParameter('EvntId')+'" id='+result.EvntId+'>Assign/Unassign Students</a></div><div><a href="EditEvent.html?EvntId='+result.EvntId+'">Edit Event</a></div><Button class="deleteevnt" value="'+getUrlParameter('EvntId')+'">Delete This Event</Button>';
						
					
					
					$('#body').html(output);
					
					
					
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
	
	//delete evnt btn
	$( ".deleteevnt" ).click(function(){
		var answer = confirm('Are you sure you want to delete this event?');
		if (answer)
		{
			//delete ques
			$.ajax({
				 type: "Delete",
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId'),
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
	//delete evnt btn end
	
});