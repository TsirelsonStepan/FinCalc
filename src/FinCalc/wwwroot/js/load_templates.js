const Templates = {};

async function loadTemplates() {
	Templates["graph_group"] = await fetch("/templates/graph_group.html").then(res => res.text());
	Templates["return_ratios_group"] = await fetch("/templates/return_ratios_group.html").then(res => res.text());
	Templates["selected_asset_item"] = await fetch("/templates/selected_asset_item.html").then(res => res.text());
	Templates["other_selected_asset_item"] = await fetch("/templates/other_selected_asset_item.html").then(res => res.text());
	Templates["search_result_item"] = await fetch("/templates/search_result_item.html").then(res => res.text());
	Templates["selected_items_header"] = await fetch("/templates/selected_items_header.html").then(res => res.text());
	
	document.querySelector(".content").innerHTML = await fetch("/templates/initial_content_screen.html").then(res => res.text());
	applyTranslations();
}
loadTemplates();

document.getElementById("cv-file-en").href = "/files/Stephan Tsirelson. CV.pdf";
document.getElementById("cv-file-ru").href = "/files/Цирельсон Степан. CV.pdf";