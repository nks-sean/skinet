export class ShopParams {
  brandFilters: string[] = [];
  typeFilters: string[] = [];
  sort: string = 'name';
  pageNumber: number = 1;
  pageSize: number = 10;
  search: string = '';
}