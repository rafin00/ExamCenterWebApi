$(function(){
	
	if(localStorage.getItem('usertype')==null||localStorage.getItem('usertype')==""||localStorage.getItem('usertype')!='Teacher')
	{
		window.location.href = "../index.html";
	}
	var username = localStorage.getItem('username')
	
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
	
	var geteventresults = function () {
		
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/registrations/'+getUrlParameter('EvntId')+'/results',
			 async: false,
		headers: {    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<table border=1 cellspacing=0 cellpadding=0><tr><th>Student UserName</th><th>Result</th><th>Total Questions</th><th>Percentage</th><th></th></tr>';
					var stdusername; var res=0;var outof=0;
						for(var i=0;i<result.length;i++){
							res=result[i].Result; stdusername=result[i].StudentUserName; 
						
						//tot ques
						$.ajax({
        async: false,
		headers: {    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))  },
        url: 'http://localhost:60270/api/students/'+localStorage.getItem("username")+'/evnts/'+result[i].EvntId+'/questions',
		complete: function(xmlhttp,status){
				if(xmlhttp.status == 200)
				{	var totalresult=xmlhttp.responseJSON;
			
			
					//for(var i=0;i<result.length;i++)
					
						
						
						
						outof=totalresult.length;
						
					//console.log(xmlhttp.responseJSON);
					
					
				}
				else
				{
					$('#results').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
						//tot ques end
						
						output +='<tr><td>'+stdusername+'</td><td>'+res+'</td><td>'+outof+'</td><td>'+(res/outof)*100+'</td><td><a href="ViewEventResultScript.html?EvntId='+getUrlParameter('EvntId')+'&&UserName='+stdusername+'">View Script</a></td></tr>';
						
						}
						
					
					output+='</table>';
					console.log(output);
					$('#body1').html(output);
					
					
					
				}
				else if(xmlhttp.status==204)
				{
					$('#body').html("</br>No Results Yet.");
				}
				else
				{$('#body').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
		
		var totalqe=0;
						//totalques
						
		
	}
	
	
	var load = function(){
		
		
		$.ajax({
			url: 'http://localhost:60270/api/teachers/'+username+'/evnts/'+getUrlParameter('EvntId'),
			 async: false,
		headers: {    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output='<table border=1 cellspacing=0 cellpadding=0><tr><th>Event Name</th><th>Start Time</th><th>End Time</th><th></th><th></th></tr>';
					
						var sttime = result.EvntSdt.split('T');
						var endtime = result.EvntEdt.split('T');
						
						output +='<tr><td>'+result.EvntName+'</td><td>'+sttime[0]+' '+sttime[1]+'</td><td>'+endtime[0]+' '+endtime[1]+'</td><td></td><td></td></tr>';
						
					
					output+='</table>'
					$('#body').html(output);
					
					geteventresults();
					
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