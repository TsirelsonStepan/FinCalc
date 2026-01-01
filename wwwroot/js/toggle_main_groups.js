const groups = Array.from(document.getElementsByClassName('group'));
var currentGroup = null;

function closeGroup(group) {
    group.querySelector('.collapse-btn').classList.toggle('open', false)
    const panel = document.getElementById(group.dataset.target);
    panel.style.display = 'none';
}

function openGroup(group) {
    group.querySelector('.collapse-btn').classList.toggle('open', true)
    const panel = document.getElementById(group.dataset.target);
    panel.style.display = 'flex';
}

function manageGroups(group) {
    if (!currentGroup) {
        openGroup(group);
        currentGroup = group;
    }
    else if (group === currentGroup) {
        closeGroup(group);
        currentGroup = null;
    }
    else if (group != currentGroup) {
        closeGroup(currentGroup);
        openGroup(group);
        currentGroup = group;
    }    
}

groups.forEach(group => {
    group.querySelector(".collapse-btn").addEventListener('click', () => manageGroups(group));
})