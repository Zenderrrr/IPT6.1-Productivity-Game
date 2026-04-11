
export function formatNumber(number: number){
  return new Intl.NumberFormat('de-CH').format(number);
}
