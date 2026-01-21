var currentGroup = null;

function closeGroup(group) {
    group.querySelector('.collapse-icon').classList.toggle('open', false)
    const panel = document.getElementById(group.dataset.target);
    panel.style.display = 'none';
}

function openGroup(group) {
    group.querySelector('.collapse-icon').classList.toggle('open', true)
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