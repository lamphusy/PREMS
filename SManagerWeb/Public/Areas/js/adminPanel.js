const allDropdown = document.querySelectorAll('#sidebar .side-dropdown') 
const sidebar = document.getElementById('sidebar');
allDropdown.forEach(item =>
  {
    const a=item.parentElement.querySelector('a:first-child');
    a.addEventListener('click',function (e)
    {
      e.preventDefault();
      this.classList.toggle('active')
      item.classList.toggle('show')
    })
  })

  //profile
  const profile=document.querySelector('nav .profile')
  const imgProfile=profile.querySelector('img')
  const dropdownProfile=profile.querySelector('.profile-link')
  imgProfile.addEventListener('click',function()
  {
    dropdownProfile.classList.toggle('show')
  })
  window.addEventListener('click',function(e)
  {
    if(e.target!==imgProfile)
    {
      if(e.target!==dropdownProfile)
      {
        if(dropdownProfile.classList.contains('show'))
        {
          dropdownProfile.classList.remove('show')
        }
      }
    }
    allMenu.forEach(item=>
      {
        const icon=item.querySelector('.icon');
        const menuLink=item.querySelector('.menu-link')
        if(e.target !== icon)
        {
          if(e.target !== menuLink)
          {
            if(menuLink.classList.contains('show'))
            {
              menuLink.classList.remove('show')
            }
          }
        }
  
      })
  })
  //progress
  const allProgress =document.querySelectorAll('main .card .progress')
  allProgress.forEach(item=>{
    item.style.setProperty('--value',item.dataset.value)
  })
  //menu
  const allMenu=document.querySelectorAll('main .content-data .head .menu')
  allMenu.forEach(item=>
    {
      const icon=item.querySelector('.icon');
      const menuLink=item.querySelector('.menu-link')
      icon.addEventListener('click',function()
      {
        menuLink.classList.toggle('show');
      })

    })
  
    //hide menu in sidebar

const toggleSidebar = document.querySelector('nav .toggle-sidebar');
const allSideDivider = document.querySelectorAll('#sidebar .divider');

if(sidebar.classList.contains('hide')) {
	allSideDivider.forEach(item=> {
		item.textContent = '-'
	})
	allDropdown.forEach(item=> {
		const a = item.parentElement.querySelector('a:first-child');
		a.classList.remove('active');
		item.classList.remove('show');
	})
} else {
	allSideDivider.forEach(item=> {
		item.textContent = item.dataset.text;
	})
}

toggleSidebar.addEventListener('click', function () {
	sidebar.classList.toggle('hide');

	if(sidebar.classList.contains('hide')) {
		allSideDivider.forEach(item=> {
			item.textContent = '-'
		})

		allDropdown.forEach(item=> {
			const a = item.parentElement.querySelector('a:first-child');
			a.classList.remove('active');
			item.classList.remove('show');
		})
	} else {
		allSideDivider.forEach(item=> {
			item.textContent = item.dataset.text;
		})
	}
})
