$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Teacher')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username')
	var load = function(){
		var dt = new Date();
		var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
		var date = dt.getYear()+'-'+dt.getMonth()+'-'+dt.getDate();
		var datetime=new Date(date)+' '+time;
		
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/running',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<table border=1 cellspacing=0 cellpadding=0><tr align=center ><td colspan=5 ><a href="CreateNewEvent.html">Create New Event</a></td></tr><tr><th>Event Name</th><th>Start Time</th><th>End Time</th><th></th><th></th></tr>';
					for(var i=0;i<result.length;i++)
					{	var sttime = result[i].EvntSdt.split('T');
						var endtime = result[i].EvntEdt.split('T');
						
						output +='<tr><td>'+result[i].EvntName+'</td><td>'+sttime[0]+' '+sttime[1]+'</td><td>'+endtime[0]+' '+endtime[1]+'</td><td><a href="ViewEvent.html?EvntId='+result[i].EvntId+'">View</a></td><td><Button class="deleteevnt" value='+result[i].EvntId+'>Delete</Button></td></tr>';
						
					}
					output+='</table>'
					$('#body').html(output);
					
					
					
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>No Events.");
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
					localStorage.setItem("usertype",'');
					window.location.href = "../index.html";
	});
	
	//delete evnt btn
	$( ".deleteevnt" ).click(function(){
		console.log($(this).val());
		var answer = confirm('Are you sure you want to delete this event?'+$(this).val());
		if (answer)
		{
			//delete ques
			$.ajax({
				 type: "Delete",
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+$(this).val(),
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