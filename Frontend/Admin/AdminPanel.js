$(function(){
	
	var load = function(){
	
		$.ajax({
			url: 'http://localhost:60270/api/admin/teachers',
			 async: false,
		headers: {
    "Authorization": "Basic " + btoa(localStorage.getItem("username") + ":" + localStorage.getItem("password"))
  },
			complete: function(xmlhttp,status){
				if(xmlhttp.status==200)
				{
					var result=xmlhttp.responseJSON;
					var output="<table border=1><tr><th>UserNAme</th><th>FullName</th><th>UserType</th><th>Password</th><th></th><th></th></tr>";
					for(var i=0;i<result.length;i++)
					{	
						output +='<tr><td>'+result[i].UserName+'</td><td>'+result[i].FullName+'</td><td>'+result[i].UserType+'</td><td>'+result[i].Password+'</td><td><a href="EditTeacher.html/UserName='+result[i].UserName+'">Edit</a><th><a href="#" id='+result[i].UserName+'>Delete</a></th></td></tr>';
						
					}
					output+='</table>'
					$('#body').html(output);
					
					
					
				}
				else
				{$('#evntName').html(xmlhttp.status + ": " + xmlhttp.statusText);
					//$('#msg').html(xmlhttp.status + ": " + xmlhttp.statusText);
				}
			}
		});
	
	}
	load();
	$('#logout').click(function(){
	localStorage.setItem("username",'');
					localStorage.setItem("password",'');
					window.location.href = "../index.html";
	});
});