import { SaleType } from "./saletype.model";

export interface Edition {
    id: number;
    title: string;
    url: string;
    price: string;
    discount: string;
    lastUpdated: string;
    saleType: SaleType;
    isDeleted: boolean;
    dataId: string;
}
