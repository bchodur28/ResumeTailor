export type PagedResult<T> = {
  items: T[];
  totalCount: number;
  totalUnfilteredCount: number;
  page: number;
  pageSize: number;
};
