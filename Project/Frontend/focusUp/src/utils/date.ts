export function GetTimeFromNow(date: Date) : string {
  const now = new Date();

  const diffMilliseconds = now.getTime() - date.getTime();

  const totalSeconds = Math.floor(diffMilliseconds / 1000);

  // Total number of minutes in the difference
  const totalMinutes = Math.floor(totalSeconds / 60);

  // Total number of hours in the difference
  const totalHours = Math.floor(totalMinutes / 60);

  if(totalMinutes < 60) {
    return `Vor ${totalMinutes} min`;
  }
  if(totalHours < 24) {
    return `Vor ${totalHours} h`;
  }

  const yesterday = new Date();
  yesterday.setDate(yesterday.getDate() - 1);

  if(date.toDateString() === yesterday.toDateString()) {
    return `Gestern, ${getTime(date)}`;
  }

  if(date.getFullYear() === now.getFullYear()) {
    return `Am ${date.getDate()}. ${date.toLocaleString('de-CH', { month: 'long'})} - ${getTime(date)}`;
  }

  else{
    return `${date.getFullYear()}, ${date.getDate()}. ${date.toLocaleString('de-CH', { month: 'long'})} - ${getTime(date)}`;
  }
}

function getTime(date: Date) : string{
  return `${formatTime(date.getHours())}:${formatTime(date.getMinutes())} Uhr`;
}

export function formatTime(value: number) : string{
  return value.toString().padStart(2, '0');
}

export function formatDate(date: Date | string) : string {
  const d = new Date(date);
  const day = String(d.getDate()).padStart(2, '0');
  const month = String(d.getMonth() + 1).padStart(2, '0');
  const year = d.getFullYear();
  return `${year}-${month}-${day}`;
}
