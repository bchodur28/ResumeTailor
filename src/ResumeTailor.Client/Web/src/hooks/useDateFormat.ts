import { useAccount } from "../contexts/AccountContext";
import { DateFormat, formatDate } from "../models/common/DateFormat";

export const useDateFormat = () => {
  const { account } = useAccount();

  const dateFormat = account?.dateFormat ?? DateFormat.MMM_YYYY;

  return {
    dateFormat,
    formatDate: (date: string | null | undefined) =>
      formatDate(date, dateFormat),
  };
};
