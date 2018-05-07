$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Teacher')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username')
	
	var load = function(){
		
		
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/finished',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<table border=1 cellspacing=0 cellpadding=0><tr><th>Event Name</th><th>Start Time</th><th>End Time</th><th></th><th></th></tr>';
					for(var i=0;i<result.length;i++)
					{	var sttime = result[i].EvntSdt.split('T');
						var endtime = result[i].EvntEdt.split('T');
						
						output +='<tr><td>'+result[i].EvntName+'</td><td>'+sttime[0]+' '+sttime[1]+'</td><td>'+endtime[0]+' '+endtime[1]+'</td><td><a href="ViewEventResult.html?EvntId='+result[i].EvntId+'">View Result</a></td><td></td></tr>';
						
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
	
	
});