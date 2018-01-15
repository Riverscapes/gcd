// Custom JS Goes HERE
$(document).ready(function (){

	/**
	 * Create a tree out of a flat path recursively.
	 * @param  {[type]}
	 * @param  {[type]}
	 * @param  {[type]}
	 * @return {[type]}
	 */
	// Now place each item in the tree
	function treeize(pages) {
		var t = { leaves: [], branches: [] }
		
		for (i in pages) {
			
			var urlArr = pages[i].url.split('/');
			var currLevel = urlArr.shift();
			var pointer = t;

			// For some reason the GH Pages jekyll detects style.css as a page.
			if (urlArr[0] === "assets") continue

			// Now loop until we're deep enough
			while (urlArr.length > 1) {
				var key = urlArr[0].length == 0 ? "EMPTY" : urlArr[0];
				key = key.replace("_"," ").replace("%20", " ");
				var newDir = null;

				var thebranch = pointer.branches.filter(br => br.key == key)
				if (thebranch.length > 0){
					newDir = thebranch[0];
				}
				else {
					newDir = { key: key, leaves: [], branches: [] }
					pointer.branches.push(newDir);					
				}
				pointer = newDir;
				currLevel = urlArr.shift();
			}
			if (urlArr[0] == "index.html" || urlArr[0] == ""){
				pointer.title = pages[i].title;
				pointer.index = pages[i];
			}
			else{
				pointer.title = pointer.key;
				pointer.leaves.push(pages[i]);				
			}			
		}	
		traverseandsort(t)
		return t
	}

	/**
	 * recursively sort a tree by weight then by alphabet
	 * @param  {[type]}
	 * @return {[type]}
	 */
	function traverseandsort(t) {
		// Now sort branches at this level
		t.branches.sort(function(a,b) {
			a.weight = a.index && a.index.weight ? parseInt(a.index.weight) : 999;
			b.weight = b.index && b.index.weight ? parseInt(b.index.weight) : 999;

			if (a.weight == b.weight) {
				return a.title < b.title ? -1 : (a.title > b.title ? 1 : 0);
			}
			else {
				return a.weight < b.weight ? -1 : (a.weight > b.weight ? 1 : 0);
			}
			return true;
		});

		// Sort the leaves by weight and then by title
		t.leaves.sort(function(a, b) {

			a.weight = parseInt(a.weight) || 999;
			b.weight = parseInt(b.weight) || 999;
			
			if (a.weight == b.weight) {
				return a.title < b.title ? -1 : (a.title > b.title ? 1 : 0);
			}
			else {
				return a.weight < b.weight ? -1 : (a.weight > b.weight ? 1 : 0);
			}
		});


		// Now go find more branches to sort their leaves
		for (br in t.branches) {
			traverseandsort(t.branches[br])
		}
	}	

	/**
	 * Turn a tree structure from SiteSettings.topmenu into a foundation topbar
	 * @param  {[type]}
	 * @return {[type]}
	 */
	function topbarize() {
		// The Mobile menu
		$topbarContainer = $('<div></div>')
		
		if (!SiteSettings.topmenu || SiteSettings.topmenu.length < 1){
			// No menu found or there was a problem
			$mobilediv = $('<div class="title-bar hide-for-medium"></div>');
			$mobilediv.append($('<div class="title-bar-title"><a href="' + NAVHome + '/">'+NAVTitle+'</a></div>'))
			$topbarContainer.append($mobilediv)
			return $topbarContainer						
		}

		var tree = SiteSettings.topmenu

		// Otherwise we get a proper menu
		$mobilediv = $('<div class="title-bar" data-responsive-toggle="responsive-menu" data-hide-for="medium"></div>');
		$mobilediv.append($('<button class="menu-icon" type="button" data-toggle="responsive-menu"></button>'))
		$mobilediv.append($('<div class="title-bar-title"><a href="' + NAVHome + '/">'+NAVTitle+'</a></div>'))
		
		$topbarContainer.append($mobilediv)

		var $topbar = $('<div class="top-bar" id="responsive-menu"></div>');
		var $topbarleft = $('<div class="top-bar-left"></div>');
		$topbar.append($topbarleft);
		
		function menutraverse(t, first=false) {
			// First time round
			var $mUL = $('<ul class="submenu menu vertical" data-submenu></ul>');
			if (first){
				var $mUL = $('<ul class="dropdown menu" data-dropdown-menu></ul>');
				var $title = $('<li class="show-for-medium"><a href="'+NAVHome+'/">'+NAVTitle+'</li>');
				$mUL.append($title);
			}
		
			function urlize(item){
				var newurl = "#";
				var target = "";
				var title = item.title;
				if (item.url){
					// Is the URL absolute or relative?
					if (item.url.indexOf("http") != 0) {
						newurl = NAVHome + '/' + item.url;	
					}
					else{
						newurl = item.url;
						target = 'target="_blank"';							
					}	
				}				
				return $('<a href="' + newurl + '" ' + target + '>' + title + '</a>');
			}

			// Loop over the immediate children
			for (cind in t) {
				// Now go find more branches to sort their leaves m656
				if (t[cind].children && t[cind].children.length > 0){

					var $li = $('<li class="has-submenu"></li>');
					$li.append(urlize(t[cind]))
					$li.append(menutraverse(t[cind].children));
					$mUL.append($li);
				}
				else {
					var $mLi = $('<li></li>');
					$mLi.append(urlize(t[cind]))
					$mUL.append($mLi);
				}
			}

			return $mUL;			
		}

		$topbarleft.append(menutraverse(tree, true));
		$topbarContainer.append($topbar)
		return $topbarContainer
	}

	/**
	 * Turn a tree structure from treeize into a foundation sidebar accordion 
	 * @param  {[type]}
	 * @return {[type]}
	 */
	function accordionize(t, $mUL) {
		// The first time we have to build the ul
		if (!$mUL) {
			$mUL = $('<ul id="topmenu" class="vertical menu accordion-menu" data-accordion-menu data-submenu-toggle="true"></ul>');
			// If we've elected to have a home item then use it
			try {
				if (SiteSettings.sideMenu.homeItem === true){
					$li = $('<li class="leaf home"><a href="' + NAVHome + '/"><i class="icon"/>Home</a></li>');
					$mUL.append($li);
				}
			} catch (error) {}
		}
		// Now go find more branches to sort their leaves
		for (lf in t.leaves) {
			$li = $('<li class="leaf page"><a href="' + t.leaves[lf].absurl + '"><i class="icon"/>' + t.leaves[lf].title + '</a></li>');
			var extSplit = t.leaves[lf].url.split('.');
			if (extSplit.length == 1 || extSplit[extSplit.length -1] == "html") 
				$mUL.append($li);
		}				
		// Now go find more branches to sort their leaves
		for (brind in t.branches) {
			$newmLi = $('<li class="branch"></li>');
			var br = t.branches[brind];

			if (br.index){
				$newmA = $('<a class="reallink" href="' + br.index.absurl + '"><i class="icon"/>' + br.index.title + '</a>');
			}
			else{
				var title = br.title || br.key;
				$newmA = $('<a class="nolink" href="#"><i class="icon"/>'+ title +'</a>');
			}
			$newmUl = $('<ul class="menu vertical nested"></ul>');
			$newmLi.append($newmA);
			$newmLi.append($newmUl);
			$mUL.append($newmLi);
			accordionize(br, $newmUl);
		}
		return $mUL;			
	}

	/**
	 * This  is our function for expanding to the current item (page)
	 * 
	 * @param {any} $sidebar 
	 */
	function expandCurentAccordion($sidebar) {
		$menuEl = $sidebar.find("[href='"+window.location.pathname+"']");
		$menuEl.addClass('menuActive');

		$immediateChild = $menuEl.parent().find('ul');
		$immediateChild.addClass('menuActive');
		$sidebar.foundation('down', $immediateChild);
		
		menuancestry = $menuEl.parentsUntil($sidebar);
		menuancestry.each(function(key, val){
			$sidebar.foundation('down', $(val));
		})
	}

	// Do all the things to get the tree
	tree = treeize(NAVPages);
	$topbar = topbarize();
	$sidebarnav = accordionize(tree);

	$('#topbarnav').append($topbar);
	$('#sidenav').append($sidebarnav);

	// Initialize our UI framework
	$(document).foundation();
	expandCurentAccordion($sidebarnav);

	// Rewrite a few of the interactions with the menu3
	$('#topmenu i.icon').click(e => {
			e.stopPropagation();
			e.preventDefault();
			$(e.toElement).parent().siblings('button').click();
	})
	$('#topmenu li.branch a.nolink').click(e => {
		e.preventDefault();
		$(e.toElement).siblings('button').click();
})
	
	// Bind our buttons to menu actions
	if (SiteSettings.sideMenu.startExpanded){
		$sidebarnav.foundation('showAll');
	}
	$('#menuCtls #expand').click(function(){
		$sidebarnav.foundation('showAll');
	});
	$('#menuCtls #contract').click(function(){
		$sidebarnav.foundation('hideAll');
		expandCurentAccordion($sidebarnav);
	});

	$('#toc').toc();
	// $('#toc').prepend('<h4 class="show-for-medium"><span class="fa fa-file-text"></span> Page Contents:</h4>')

});
