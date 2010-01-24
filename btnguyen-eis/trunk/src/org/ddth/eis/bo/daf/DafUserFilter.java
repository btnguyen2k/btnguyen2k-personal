package org.ddth.eis.bo.daf;

import java.util.ArrayList;
import java.util.Collection;

import org.ddth.panda.utils.StringUtils;

public class DafUserFilter {
	public final static DafUserFilter DEFAULT_DAF_USER_FILTER = new DafUserFilter();

	int pageSize = 1;

	int page = 1;

	public String whereCriteria;

	public Collection<DafSortingCriteria> sortingCriteria = new ArrayList<DafSortingCriteria>();

	/**
	 * Constructs a new DafUserFilter object.
	 */
	public DafUserFilter() {
	}

	/**
	 * Gets offset of page's start.
	 * 
	 * @return int
	 */
	public int getPageOffset() {
		return (getPage() - 1) * getPageSize();
	}

	/**
	 * Gets page number.
	 * 
	 * @return int
	 */
	public int getPage() {
		return page;
	}

	/**
	 * Sets page number.
	 * 
	 * @param page
	 *            int
	 */
	public void setPage(int page) {
		if (page < 1) {
			page = 1;
		}
		this.page = page;
	}

	/**
	 * Gets page size.
	 * 
	 * @return int
	 */
	public int getPageSize() {
		return pageSize;
	}

	/**
	 * Sets page size.
	 * 
	 * @param pageSize
	 *            int
	 */
	public void setPageSize(int pageSize) {
		if (pageSize < 1) {
			pageSize = 1;
		}
		this.pageSize = pageSize;
	}

	/**
	 * Checks if filter contains sorting criteria.
	 * 
	 * @return boolean
	 */
	public boolean hasSortingCriteria() {
		return sortingCriteria.size() > 0;
	}

	/**
	 * Adds a sorting criteria.
	 * 
	 * @param sortingCriteria
	 *            DafSortingCriteria
	 */
	public void addSortingCriteria(DafSortingCriteria sortingCriteria) {
		this.sortingCriteria.add(sortingCriteria);
	}

	/**
	 * Gets soring criteria.
	 * 
	 * @return Collection<DafSortingCriteria>
	 */
	public Collection<DafSortingCriteria> getSortingCriteria() {
		return sortingCriteria;
	}

	/**
	 * Checks if filter contains WHERE criteria.
	 * 
	 * @return boolean
	 */
	public boolean hasWhereCriteria() {
		return !StringUtils.isEmpty(whereCriteria);
	}

	/**
	 * Gets WHERE criteria.
	 * 
	 * @return String
	 */
	public String getWhereCriteria() {
		return this.whereCriteria;
	}

	/**
	 * Sets WHERE criteria.
	 * 
	 * @param whereCriteria
	 *            String
	 */
	public void setWhereCriteria(String whereCriteria) {
		this.whereCriteria = whereCriteria;
	}
}
